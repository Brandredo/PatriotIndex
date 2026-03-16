using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Telemetry;
using PatriotIndex.Domain.Transformers;

namespace PatriotIndex.Domain.Jobs;


public class SeasonalStatsJob(SportsApiClient apiClient, ILogger<SeasonalStatsJob> logger, SyncLogRepository syncLogRepository, StatsRepository statsRepository)
{
    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid teamId, SeasonInput seasonInput, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting SeasonalStatsJob for team {TeamId} ({Year}/{SeasonType})", teamId, seasonInput.SeasonYear, seasonInput.SeasonType);

        using var activity = PatriotIndexTelemetry.Source.StartActivity("SeasonalStatsJob.RunAsync", ActivityKind.Internal);
        activity?.SetTag("season.stats.job.id", $"{seasonInput.SeasonYear}.{seasonInput.SeasonType},{teamId}");

        var entityId = $"SeasonalStats:{teamId}:{seasonInput.SeasonYear}:{seasonInput.SeasonType}";

        try
        {
            string? data = null;

            var rawResponse = await syncLogRepository.IsDuplicateEntry(entityId, cancellationToken);

            if (rawResponse == null)
            {
                var log = new SyncLog
                {
                    EntityType = $"SeasonalStats:{teamId}:{seasonInput.SeasonYear}:{seasonInput.SeasonType}",
                    StartedAt = DateTime.UtcNow,
                    Status = "Pending",
                    RecordCount = 0,
                };

                var respId = await syncLogRepository.InsertEntry(log, cancellationToken);

                // 1. call the api to get the game data
                data = await apiClient.GetAsync($"seasons/{seasonInput.SeasonYear}/{seasonInput.SeasonType}/teams/{teamId}/statistics.json", cancellationToken);

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

            var sst = new SeasonalStatsTransformer(JsonSerializer.Deserialize<SeasonalStatsApiResponse>(data));
            var (team, players, playerStats) = sst.Transform();
            activity?.AddEvent(new ActivityEvent("transform.complete"));

            if(team == null || players == null || playerStats == null) throw new Exception("stats is null");

            await statsRepository.SaveAsync(team, players, playerStats, cancellationToken);

            activity?.AddEvent(new ActivityEvent("db.save.complete"));

            logger.LogInformation("SeasonalStatsJob completed for team {TeamId}", teamId);

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception e)
        {
            activity?.AddException(e);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "SeasonalStatsJob failed for team {TeamId}", teamId);
            throw;
        }

    }


}

public record SeasonInput
{
    public Guid? SeasonId { get; set; }
    public string SeasonType { get; set; } = "REG";
    public int SeasonYear { get; set; } = 2025;
}
