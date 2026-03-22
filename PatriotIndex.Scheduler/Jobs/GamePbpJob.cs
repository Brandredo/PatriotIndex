using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using PatriotIndex.Domain.Services;

namespace PatriotIndex.Scheduler.Jobs;

public class GamePbpJob(
    SportsApiClient apiClient,
    SyncLogRepository syncLogRepository,
    GamesRepository gamesRepository,
    ILogger<GamePbpJob> logger,
    IBackgroundJobClient backgroundJobClient)
{
    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid gameId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting GamePbpJob for game {GameId}", gameId);

        using var activity = PatriotIndexTelemetry.Source.StartActivity("GamePbpJob.RunAsync", ActivityKind.Internal);
        activity?.SetTag("game.id", gameId);

        var entityId = $"GamePbp:{gameId}";

        try
        {
            string? data = null;

            var rawResponse = await syncLogRepository.IsDuplicateEntry(entityId, cancellationToken);

            if (rawResponse == null)
            {
                var log = new SyncLog
                {
                    EntityType = entityId,
                    StartedAt = DateTime.UtcNow,
                    Status = "Pending",
                    RecordCount = 0
                };

                var respId = await syncLogRepository.InsertEntry(log, cancellationToken);

                // 1. call the api to get the game data
                data = await apiClient.GetAsync($"games/{gameId}/pbp.json", cancellationToken);

                activity?.AddEvent(new ActivityEvent("http.fetch.complete"));

                // 2. persist the api response in the database
                await syncLogRepository.UpdateEntry(respId, entry =>
                {
                    entry.RawResponse = JsonDocument.Parse(data);
                    entry.CompletedAt = DateTime.UtcNow;
                    entry.Status = "Success";
                    entry.RecordCount = 1;
                }, cancellationToken);

                activity?.AddEvent(new ActivityEvent("sync.log.saved"));
            }
            else
            {
                data = rawResponse.RootElement.GetRawText();
            }

            if (string.IsNullOrWhiteSpace(data)) throw new Exception("game data is null");

            // 3. transform the data into a model
            var gt = new GamePbpTransformer(data);
            var pbp = gt.Transform();
            if (pbp == null) throw new Exception("game tranformed is null");
            activity?.AddEvent(new ActivityEvent("transform.complete"));

            // 4. persist the model in the database
            await gamesRepository.SaveAsync(pbp, cancellationToken);

            activity?.AddEvent(new ActivityEvent("db.save.complete"));

            logger.LogInformation("GamePbpJob completed for game {GameId}", gameId);

            // start a job to update the seasonal stats of teams of this game
            await UpdateSeasonalStats(pbp, cancellationToken);

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception e)
        {
            activity?.AddException(e);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "GamePbpJob failed for game {GameId}", gameId);
            throw;
        }
    }


    private async Task UpdateSeasonalStats(Game game, CancellationToken cancellationToken)
    {
        var hId = game.HomeTeamId ?? throw new Exception("home team id is null");
        var aId = game.AwayTeamId ?? throw new Exception("away team id is null");

        // get the current season from configuration
        var seasonId = await syncLogRepository.GetCurrentSeasonId(cancellationToken);

        //backgroundJobClient.Enqueue<SeasonalStatsJob>(job => job.RunAsync(hId, new SeasonInput { SeasonId = seasonId }, cancellationToken)); // home team
        //backgroundJobClient.Enqueue<SeasonalStatsJob>(job => job.RunAsync(aId, new SeasonInput { SeasonId = seasonId }, cancellationToken)); // away team
        backgroundJobClient.Enqueue<GameSummaryStatsJob>(job =>
            job.RunAsync(game.Id, cancellationToken)); // game summary/box-score stats
    }
}