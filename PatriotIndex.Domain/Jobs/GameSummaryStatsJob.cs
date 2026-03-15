using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Transformers;

namespace PatriotIndex.Domain.Jobs;

public class GameSummaryStatsJob(
    SportsApiClient apiClient,
    SyncLogRepository syncLogRepository,
    GameStatsRepository gameStatsRepository,
    ILogger<GameSummaryStatsJob> logger)
{
    private static readonly ActivitySource _tracer = new("GameSummaryStatsJob");

    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid gameId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting GameSummaryStatsJob for game {GameId}", gameId);

        using var activity = _tracer.StartActivity("GameSummaryStatsJob.RunAsync");
        activity?.SetTag("game.id", gameId);

        var log = new SyncLog
        {
            EntityType  = $"GameSummaryStats:{gameId}",
            StartedAt   = DateTime.UtcNow,
            Status      = "Pending",
            RecordCount = 0,
        };

        try
        {
            var respId = await syncLogRepository.InsertEntry(log, cancellationToken);

            var data = await apiClient.GetAsync($"games/{gameId}/statistics", cancellationToken);

            await syncLogRepository.UpdateEntry(respId, entry =>
            {
                entry.RawResponse = JsonDocument.Parse(data);
                entry.CompletedAt = DateTime.UtcNow;
                entry.Status      = "Success";
                entry.RecordCount = 1;
            }, cancellationToken);

            var (teamStats, playerStats) = new GameSummaryStatsTransformer(data).Transform();

            await gameStatsRepository.SaveAsync(teamStats, playerStats, cancellationToken);

            activity?.SetStatus(ActivityStatusCode.Ok);
            logger.LogInformation(
                "GameSummaryStatsJob completed for game {GameId}: {T} team rows, {P} player rows.",
                gameId, teamStats.Count, playerStats.Count);
        }
        catch (Exception e)
        {
            activity?.SetTag("error", e.Message);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "GameSummaryStatsJob failed for game {GameId}", gameId);
            throw;
        }
    }
}
