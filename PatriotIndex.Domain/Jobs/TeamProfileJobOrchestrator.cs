using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace PatriotIndex.Domain.Jobs;

public class TeamProfileJobOrchestrator(IBackgroundJobClient backgroundJobClient, IDbContextFactory<PatriotIndexDbContext> dbContextFactory)
{
    public async Task RunAsync()
    {
        // var teams = await _teamRepository.GetAllAsync(); // returns all 32 teams
        //
        // foreach (var team in teams)
        // {
        //     backgroundJobClient.Enqueue<TeamProfileJob>(job => job.RunAsync(team.Id, CancellationToken.None));
        // }
        backgroundJobClient.Enqueue(() => Console.WriteLine("Team Profile Orchestrator Job started"));
        
    }
}