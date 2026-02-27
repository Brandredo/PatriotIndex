using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Repository;

namespace PatriotIndex.Domain.Jobs;

public class TeamProfileJobOrchestrator(TeamsRepository teamRepository, IBackgroundJobClient backgroundJobClient, ILogger<TeamProfileJobOrchestrator> logger)
{
    public async Task RunAsync()
    {
        logger.LogInformation("Starting Team Profile Job Orchestrator");
        
        var teamsIds = await teamRepository.GetTeamIdsAsync(); // returns all 32 team ids
        
        foreach (var id in teamsIds)
        {
            backgroundJobClient.Enqueue<TeamProfileJob>(job => job.RunAsync(id, CancellationToken.None));
        }
        logger.LogInformation("Team Profile Job Orchestrator completed");
    }
}