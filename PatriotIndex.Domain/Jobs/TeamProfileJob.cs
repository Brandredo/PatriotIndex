using System.Text.Json;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;

namespace PatriotIndex.Domain.Jobs;

public class TeamProfileJob(SportsApiClient apiClient, SyncLogRepository syncLogRepository, ILogger<TeamProfileJob> logger)
{

    
    public async Task RunAsync(Guid teamId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Team Profile Job");
        
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

        // 2. persist the api response in the database
        await syncLogRepository.InsertEntry(log, cancellationToken);

        // 3. transform the data into a model

        // 4. persist the model in the database

    }
    
}