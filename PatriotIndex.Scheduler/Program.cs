using System.Threading.RateLimiting;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Jobs;
using PatriotIndex.Domain.Services;
using PatriotIndex.Infrastructure.Data;
using PatriotIndex.Infrastructure.Repositories;
using PatriotIndex.ServiceDefaults;
using Polly;
using CurrentWeekScheduleJob = PatriotIndex.Scheduler.Jobs.CurrentWeekScheduleJob;
using GamePbpJob = PatriotIndex.Scheduler.Jobs.GamePbpJob;
using GamePbpJobOrchestrator = PatriotIndex.Scheduler.Jobs.GamePbpJobOrchestrator;
using GameSummaryStatsJob = PatriotIndex.Scheduler.Jobs.GameSummaryStatsJob;
using SeasonalStatsJob = PatriotIndex.Scheduler.Jobs.SeasonalStatsJob;
using SeasonalStatsJobOrchestrator = PatriotIndex.Scheduler.Jobs.SeasonalStatsJobOrchestrator;
using SyncLogRepository = PatriotIndex.Domain.Repository.SyncLogRepository;
using TeamProfileJob = PatriotIndex.Scheduler.Jobs.TeamProfileJob;
using TeamProfileJobOrchestrator = PatriotIndex.Scheduler.Jobs.TeamProfileJobOrchestrator;

namespace PatriotIndex.Scheduler;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        builder.AddNpgsqlDbContext<PatriotIndexDbContext>("PostgresDb", configureDbContextOptions: options =>
        {
            options.UseSnakeCaseNamingConvention();
            options.EnableDetailedErrors();
        });

        // Hangfire
        builder.Services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("PostgresDb") ??
                                       throw new ArgumentNullException("Hangfire connection string is missing");
                options.UseNpgsqlConnection(connectionString);
            }, new PostgreSqlStorageOptions
            {
                SchemaName = "hangfire"
                //UseNativeDatabaseTransactions = true,
            }));

        builder.Services.AddHangfireServer(options =>
        {
            options.WorkerCount = 1; // Set the number of workers to 1 to ensure sequential execution
        });

        // Register job classes
        builder.Services.AddScoped<TeamProfileJobOrchestrator>();
        builder.Services.AddScoped<CurrentWeekScheduleJob>();
        builder.Services.AddScoped<TeamProfileJob>();
        builder.Services.AddScoped<GamePbpJob>();
        builder.Services.AddScoped<SeasonalStatsJob>();
        builder.Services.AddScoped<SeasonalStatsJobOrchestrator>();
        builder.Services.AddScoped<GamePbpJobOrchestrator>();
        builder.Services.AddScoped<GameSummaryStatsJob>();

        // Register repository classes
        builder.Services.AddScoped<TeamProfileRepository>();
        builder.Services.AddScoped<GameStatisticsRepository>();
        builder.Services.AddScoped<SyncLogRepository>();
        builder.Services.AddScoped<SeasonalStatisticsRepository>();
        builder.Services.AddScoped<SeasonsRepository>();
        builder.Services.AddScoped<GamePlayByPlayRepository>();

        // Register services
        builder.Services.AddHttpClient<SportsApiClient>()
            .AddResilienceHandler("token-limiter", builder =>
            {
                builder.AddRateLimiter(new TokenBucketRateLimiter(
                    new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 1,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(2),
                        TokensPerPeriod = 1,
                        QueueLimit = 50
                    }));
            });

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = []
        });

        // Misfire modes
        // Relaxed: (Default) If a job was missed, fire it once immediately on startup
        // Ignorable: If a job was missed, skip it and wait for the next scheduled time
        // Strict: Fire the job for every missed occurrence (use with caution)


        // Hangfire recurring jobs
        RecurringJob.AddOrUpdate<TeamProfileJobOrchestrator>(
            "team_profile-orchestrator",
            job => job.RunAsync(),
            "0 6 * * *",
            new RecurringJobOptions
            {
                MisfireHandling = MisfireHandlingMode.Ignorable // Key setting
            });

        RecurringJob.AddOrUpdate<CurrentWeekScheduleJob>(
            "week-schedule-orchestrator",
            job => job.RunAsync(),
            "0 0 * * *",
            new RecurringJobOptions
            {
                MisfireHandling = MisfireHandlingMode.Ignorable // Key setting
            });

        RecurringJob.AddOrUpdate<SeasonalStatsJobOrchestrator>(
            "seasonal_stats-orchestrator",
            job => job.RunAsync(),
            "0 6 * * *",
            new RecurringJobOptions
            {
                MisfireHandling = MisfireHandlingMode.Ignorable // Key setting
            });

        RecurringJob.AddOrUpdate<GamePbpJobOrchestrator>(
            "game-pbp-orchestrator",
            job => job.RunAsync(),
            "0 6 * * *",
            new RecurringJobOptions
            {
                MisfireHandling = MisfireHandlingMode.Ignorable // Key setting
            });

        // -----------------------

        await app.RunAsync();
    }
}