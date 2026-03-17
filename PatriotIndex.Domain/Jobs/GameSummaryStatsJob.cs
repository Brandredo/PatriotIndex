using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Telemetry;
using PatriotIndex.Domain.Transformers;

namespace PatriotIndex.Domain.Jobs;

public class GameSummaryStatsJob(
    SportsApiClient apiClient,
    SyncLogRepository syncLogRepository,
    GameStatsRepository gameStatsRepository,
    ILogger<GameSummaryStatsJob> logger)
{
    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid gameId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting GameSummaryStatsJob for game {GameId}", gameId);

        using var activity = PatriotIndexTelemetry.Source.StartActivity("GameSummaryStatsJob.RunAsync", ActivityKind.Internal);
        activity?.SetTag("game.id", gameId);

        var entityId = $"GameSummaryStats:{gameId}";

        try
        {
            string? data = null;

            var rawResponse = await syncLogRepository.IsDuplicateEntry(entityId, cancellationToken);

            if (rawResponse == null)
            {
                var log = new SyncLog
                {
                    EntityType  = entityId,
                    StartedAt   = DateTime.UtcNow,
                    Status      = "Pending",
                    RecordCount = 0,
                };

                var respId = await syncLogRepository.InsertEntry(log, cancellationToken);

                data = await apiClient.GetAsync($"games/{gameId}/statistics", cancellationToken);

                activity?.AddEvent(new ActivityEvent("http.fetch.complete"));

                await syncLogRepository.UpdateEntry(respId, entry =>
                {
                    entry.RawResponse = JsonDocument.Parse(data);
                    entry.CompletedAt = DateTime.UtcNow;
                    entry.Status      = "Success";
                    entry.RecordCount = 1;
                }, cancellationToken);

                activity?.AddEvent(new ActivityEvent("sync.log.saved"));
            }
            else
            {
                data = rawResponse.RootElement.GetRawText();
            }

            if (string.IsNullOrWhiteSpace(data)) throw new Exception("game data is null");

            var (teamStats, playerStats, players) = new GameSummaryStatsTransformer(data, logger).Transform();

            activity?.AddEvent(new ActivityEvent("transform.complete"));

            await gameStatsRepository.SaveAsync(teamStats, playerStats, players, cancellationToken);

            activity?.AddEvent(new ActivityEvent("db.save.complete"));
            activity?.SetStatus(ActivityStatusCode.Ok);
            logger.LogInformation(
                "GameSummaryStatsJob completed for game {GameId}: {T} team rows, {P} player rows.",
                gameId, teamStats.Count, playerStats.Count);
        }
        catch (Exception e)
        {
            activity?.AddException(e);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "GameSummaryStatsJob failed for game {GameId}", gameId);
            throw;
        }
    }
}
