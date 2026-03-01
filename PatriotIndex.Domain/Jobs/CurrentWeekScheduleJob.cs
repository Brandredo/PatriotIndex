using System.Text.Json;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Transformers;

namespace PatriotIndex.Domain.Jobs;

public class CurrentWeekScheduleJob(IBackgroundJobClient backgroundJobClient, SportsApiClient apiClient, ILogger<CurrentWeekScheduleJob> logger, SyncLogRepository syncLogRepository, GamesRepository gamesRepository )
{
    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        
        logger.LogInformation("Starting Current Week Schedule Job");

        try
        {
            
            // 1. call the api to get the team profile data
            var log = new SyncLog();
            log.StartedAt = DateTime.UtcNow;
            
            var data = await apiClient.GetAsync($"/games/current_week/schedule.json", cancellationToken);
            log.CompletedAt = DateTime.UtcNow;
            log.Status = "Completed";
            log.RecordCount = 1;
            log.RawResponse = JsonDocument.Parse(data);
            
            // 2. persist the api response in the database
            await syncLogRepository.InsertEntry(log, cancellationToken);

            var week = new CurrentWeekTransformer(data);
            var games = week.Transform();
            if(games == null) throw new Exception("List of games is null");
            var list = games.ToList();

            await gamesRepository.SaveAsync(list, cancellationToken);
            
            foreach (var game in list)
                backgroundJobClient.Enqueue<GamePbpJob>(job => job.RunAsync(game.Id, CancellationToken.None));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
}