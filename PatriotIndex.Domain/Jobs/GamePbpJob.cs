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
    SyncLogRepository syncLogRepository, GamesRepository gamesRepository, ILogger<GamePbpJob> logger)
{
    
    private static readonly ActivitySource _tracer = new("GamePbpJob");

    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid gameId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Game Pbp Job");

        using var activity = _tracer.StartActivity("GamePbpJob.RunAsync");
        try
        {


            activity?.SetTag("game.id", gameId);

            var log = new SyncLog
            {
                //Id = default,
                EntityType = $"GamePbp:{gameId}",
                StartedAt = DateTime.UtcNow,
                CompletedAt = null,
                Status = null,
                RecordCount = 0,
                ErrorMessage = null,
                RawResponse = null
            };

            // 1. call the api to get the game data
            var data = await apiClient.GetAsync($"games/{gameId}/pbp.json", cancellationToken);
            log.CompletedAt = DateTime.UtcNow;
            log.Status = "Completed";
            log.RecordCount = 1;
            log.RawResponse = JsonDocument.Parse(data);

            activity?.SetTag("game.pbp.job.http", "Completed");

            // 2. persist the api response in the database
            await syncLogRepository.InsertEntry(log, cancellationToken);

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
}