using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Transformers;

namespace PatriotIndex.Domain.Jobs;

public class GamePbpJob(SportsApiClient apiClient,
    SyncLogRepository syncLogRepository, GamesRepository gamesRepository, ILogger<GamePbpJob> logger, IBackgroundJobClient backgroundJobClient)
{
    
    private static readonly ActivitySource _tracer = new("GamePbpJob");

    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid gameId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Game Pbp Job");

        using var activity = _tracer.StartActivity("GamePbpJob.RunAsync");
        
        var log = new SyncLog
        {
            //Id = default,
            EntityType = $"GamePbp:{gameId}",
            StartedAt = DateTime.UtcNow,
            CompletedAt = null,
            Status = "Pending",
            RecordCount = 0,
            ErrorMessage = null,
            RawResponse = null
        };
        
        try
        {
            activity?.SetTag("game.id", gameId);
            
            var respId = await syncLogRepository.InsertEntry(log, cancellationToken);
            
            // 1. call the api to get the game data
            var data = await apiClient.GetAsync($"games/{gameId}/pbp.json", cancellationToken);

            activity?.SetTag("game.pbp.job.http", "Completed");

            // 2. persist the api response in the database
            await syncLogRepository.UpdateEntry(respId, entry =>
            {
                entry.RawResponse = JsonDocument.Parse(data);
                entry.CompletedAt = DateTime.UtcNow;
                entry.Status = "Success";
                entry.RecordCount = 1;
            }, cancellationToken);

            activity?.SetTag("game.pbp.job.cached", "Completed");

            // 3. transform the data into a model
            var gt = new GamePbpTransformer(data);
            var pbp = gt.Transform();
            if(pbp == null) throw new Exception("game tranformed is null");
            activity?.SetTag("game.pbp.job.model", "Completed");

            // 4. persist the model in the database
            await gamesRepository.SaveAsync(pbp, cancellationToken);

            activity?.SetTag("game.pbp.job.db", "Completed");
            logger.LogInformation("Game Pbp Job completed");
            
            // start a job to update the seasonal stats of teams of this game
            await UpdateSeasonalStats(pbp, cancellationToken);
            
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception e)
        {
            activity?.SetTag("game.pbp.job.error", e.Message);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "Game Pbp Job failed");
            throw;
        }
    }
    
    
    private async Task UpdateSeasonalStats(Game game, CancellationToken cancellationToken)
    {
        var hId = game.HomeTeamId ?? throw new Exception("home team id is null");
        var aId = game.AwayTeamId ?? throw new Exception("away team id is null");
        
        // get the current season from configuration
        var seasonId = await syncLogRepository.GetCurrentSeasonId(cancellationToken);
        
        backgroundJobClient.Enqueue<SeasonalStatsJob>(job => job.RunAsync(hId, new SeasonInput { SeasonId = seasonId }, cancellationToken));
        backgroundJobClient.Enqueue<SeasonalStatsJob>(job => job.RunAsync(aId, new SeasonInput { SeasonId = seasonId }, cancellationToken));
        backgroundJobClient.Enqueue<GameSummaryStatsJob>(job => job.RunAsync(game.Id, cancellationToken));
    }
}