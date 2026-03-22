using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;

namespace PatriotIndex.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Applying database migrations...");

        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PatriotIndexDbContext>();

        await db.Database.MigrateAsync(stoppingToken);

        logger.LogInformation("Migrations applied successfully.");

        hostApplicationLifetime.StopApplication();
    }
}