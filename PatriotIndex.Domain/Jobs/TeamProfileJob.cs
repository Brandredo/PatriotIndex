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

public class TeamProfileJob(
    SportsApiClient apiClient,
    SyncLogRepository syncLogRepository,
    TeamsRepository teamsRepository,
    ILogger<TeamProfileJob> logger)
{
    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid teamId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting TeamProfileJob for team {TeamId}", teamId);

        using var activity = PatriotIndexTelemetry.Source.StartActivity("TeamProfileJob.RunAsync", ActivityKind.Internal);
        activity?.SetTag("team.id", teamId);

        var entityId = $"TeamProfile:{teamId}";

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
                    RecordCount = 0,
                };

                var respId = await syncLogRepository.InsertEntry(log, cancellationToken);

                // 1. call the api to get the team profile data
                data = await apiClient.GetAsync($"teams/{teamId}/profile.json", cancellationToken);

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

            if (string.IsNullOrWhiteSpace(data)) throw new Exception("team profile data is null");

            // 3. transform the data into a model
            var tpt = new TeamProfileTransformer(data);
            var team = tpt.Transform();

            activity?.AddEvent(new ActivityEvent("transform.complete"));

            // 4. persist the model in the database
            await teamsRepository.SaveOrUpdateAsync(team, cancellationToken);

            activity?.AddEvent(new ActivityEvent("db.save.complete"));
            logger.LogInformation("TeamProfileJob completed for team {TeamId}", teamId);
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception e)
        {
            activity?.AddException(e);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "TeamProfileJob failed for team {TeamId}", teamId);
            throw;
        }
    }
}
