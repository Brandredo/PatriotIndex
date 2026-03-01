using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Transformers;

namespace PatriotIndex.Domain.Jobs;

[AutomaticRetry(Attempts = 0)]
public class TeamProfileJob(
    SportsApiClient apiClient,
    SyncLogRepository syncLogRepository,
    TeamsRepository teamsRepository,
    ILogger<TeamProfileJob> logger)
{
    private static readonly ActivitySource _tracer = new("MyApp.TeamProfileJobOrchestrator");


    public async Task RunAsync(Guid teamId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Team Profile Job");

        using var activity = _tracer.StartActivity("TeamProfileJob.RunAsync");
        try
        {
            // BEFORE THE JOB RUNS, CHECK IN THE SYNCLOG TABLE IF THE JOB ALREADY RUNS FOR THIS TEAM BUT FAILED RECENTLY
            //var result =

            activity?.SetTag("team.id", teamId);

            var log = new SyncLog
            {
                //Id = default,
                EntityType = $"TeamProfile:{teamId}",
                StartedAt = DateTime.UtcNow,
                CompletedAt = null,
                Status = null,
                RecordCount = 0,
                ErrorMessage = null,
                RawResponse = null
            };

            // 1. call the api to get the team profile data
            var data = await apiClient.GetAsync($"teams/{teamId}/profile.json", cancellationToken);
            log.CompletedAt = DateTime.UtcNow;
            log.Status = "Completed";
            log.RecordCount = 1;
            log.RawResponse = JsonDocument.Parse(data);

            activity?.SetTag("team.profile.job.http", "Completed");

            // 2. persist the api response in the database
            await syncLogRepository.InsertEntry(log, cancellationToken);

            activity?.SetTag("team.profile.job.cached", "Completed");

            // 3. transform the data into a model
            var tpt = new TeamProfileTransformer(data);
            var team = tpt.Transform();

            activity?.SetTag("team.profile.job.model", "Completed");

            // 4. persist the model in the database
            await teamsRepository.SaveOrUpdateAsync(team, cancellationToken);

            activity?.SetTag("team.profile.job.db", "Completed");
            logger.LogInformation("Team Profile Job completed");
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception e)
        {
            activity?.SetTag("team.profile.job.error", e.Message);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "Team Profile Job failed");
            throw;
        }
    }
}