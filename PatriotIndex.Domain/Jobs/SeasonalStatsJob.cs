using System.Diagnostics;
using System.Text.Json;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.Domain.Transformers;

namespace PatriotIndex.Domain.Jobs;


public class SeasonalStatsJob(SportsApiClient apiClient, ILogger<SeasonalStatsJob> logger, SyncLogRepository syncLogRepository, StatsRepository statsRepository)
{
    private static readonly ActivitySource _tracer = new("SeasonalStatsJob");
    
    [AutomaticRetry(Attempts = 0)]
    public async Task RunAsync(Guid teamId, SeasonInput seasonInput, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting Seasonal Stats Job");
        
        using var activity = _tracer.StartActivity("SeasonalStatsJob.RunAsync");
        
        var log = new SyncLog
        {
            //Id = default,
            EntityType = $"SeasonalStats:{teamId}",
            StartedAt = DateTime.UtcNow,
            CompletedAt = null,
            Status = "Pending",
            RecordCount = 0,
            ErrorMessage = null,
            RawResponse = null
        };
        
        try
        {
            activity?.SetTag("team.id", teamId);
            
            var respId = await syncLogRepository.InsertEntry(log, cancellationToken);
            
            var data = await apiClient.GetAsync($"seasons/{seasonInput.SeasonYear}/{seasonInput.SeasonType}/teams/{teamId}/statistics.json", cancellationToken);
            
            activity?.SetTag("stats.job.http", "Completed");
            
            await syncLogRepository.UpdateEntry(respId, entry =>
            {
                entry.RawResponse = JsonDocument.Parse(data);
                entry.CompletedAt = DateTime.UtcNow;
                entry.Status = "Success";
                entry.RecordCount = 1;
            }, cancellationToken);
            
            activity?.SetTag("stats.job.cached", "Completed");
            
            var sst = new SeasonalStatsTransformer(JsonSerializer.Deserialize<SeasonalStatsApiResponse>(data));
            var (team, players, playerStats) = sst.Transform();
            activity?.SetTag("stats.job.model", "Completed");

            if(team == null || players == null || playerStats == null) throw new Exception("stats is null");

            await statsRepository.SaveAsync(team, players, playerStats, cancellationToken);
            activity?.SetTag("stats.job.db", "Completed");
            logger.LogInformation("Stats Job completed");
            activity?.SetStatus(ActivityStatusCode.Ok);            
        }
        catch (Exception e)
        {
            activity?.SetTag("stats.job.error", e.Message);
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            logger.LogError(e, "Stats Job failed");
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