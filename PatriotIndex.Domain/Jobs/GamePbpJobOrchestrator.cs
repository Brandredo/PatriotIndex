using System.Diagnostics;
using Hangfire;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Repository;

namespace PatriotIndex.Domain.Jobs;

[AutomaticRetry(Attempts = 0)]
public class GamePbpJobOrchestrator(
    GamesRepository gamesRepository,
    IBackgroundJobClient backgroundJobClient,
    ILogger<GamePbpJobOrchestrator> logger)
{
    private static readonly ActivitySource _tracer = new("MyApp.GamePbpJobOrchestrator");

    public async Task RunAsync()
    {
        using var activity = _tracer.StartActivity("GamePbpJobOrchestrator.RunAsync");

        logger.LogInformation("Starting Game Pbp Job Orchestrator");

        var gameIds = await gamesRepository.GetGamesByTypeAndYearAsync("REG", 2025, CancellationToken.None); // returns all 32 team ids


        if (gameIds.Count == 0)
        {
            logger.LogInformation("No games found");
            return;
        }
        
        activity?.SetTag("team.count", gameIds.Count);
        
        foreach (var id in gameIds)
        {
            backgroundJobClient.Enqueue<GamePbpJob>(job => job.RunAsync(id, CancellationToken.None));
        }
        logger.LogInformation("Seasonal Game Pbp Job Orchestrator completed");
        activity?.SetStatus(ActivityStatusCode.Ok);
    }
    

}

