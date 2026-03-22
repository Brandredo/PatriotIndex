using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Transformers;
using PatriotIndex.Infrastructure.Repositories;

namespace PatriotIndex.Scheduler.Jobs;

public class CurrentWeekScheduleJob(
    IBackgroundJobClient backgroundJobClient,
    SportsApiClient apiClient,
    ILogger<CurrentWeekScheduleJob> logger,
    SyncLogRepository syncLogRepository,
    GameStatisticsRepository gamesRepository)
{
    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync()
    {
        logger.LogInformation("Starting Current Week Schedule Job");

        using var activity =
            PatriotIndexTelemetry.Source.StartActivity("CurrentWeekScheduleJob.RunAsync", ActivityKind.Internal);

        try
        {
            // 1. call the api to get the team profile data
            
            var log = new SyncLog();
            log.StartedAt = DateTime.UtcNow;

            var data = await apiClient.GetAsync("games/current_week/schedule.json", CancellationToken.None);

            activity?.AddEvent(new ActivityEvent("http.fetch.complete"));

            log.CompletedAt = DateTime.UtcNow;
            log.Status = "Completed";
            log.RecordCount = 1;
            log.RawResponse = JsonDocument.Parse(data);

            // 2. persist the api response in the database
            await syncLogRepository.InsertEntry(log, CancellationToken.None);

            activity?.AddEvent(new ActivityEvent("sync.log.saved"));

            var week = new CurrentWeekTransformer(data);
            var games = week.Transform();
            if (games == null) throw new Exception("List of games is null");
            var list = games.ToList();

            activity?.AddEvent(new ActivityEvent("transform.complete"));

            await gamesRepository.SaveAsync(list, CancellationToken.None);

            activity?.AddEvent(new ActivityEvent("db.save.complete"));

            foreach (var game in list)
                backgroundJobClient.Enqueue<GamePbpJob>(job => job.RunAsync(game.Id, CancellationToken.None));

            logger.LogInformation("Current Week Schedule Job completed");
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception e)
        {
            activity?.AddException(e);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "Current Week Schedule Job failed");
            throw;
        }
    }
}