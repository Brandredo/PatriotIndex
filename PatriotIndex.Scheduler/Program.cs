using System.Threading.RateLimiting;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Jobs;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Domain.Services;
using PatriotIndex.ServiceDefaults;
using Polly;

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

        // builder.EnrichNpgsqlDbContext<PatriotIndexDbContext>(settings =>
        // {
        //     settings.Ena
        //     settings.DisableRetry = true;
        //     settings.CommandTimeout = 30;
        // });

        // Add a database
        // builder.Services.AddDbContextFactory<PatriotIndexDbContext>(options =>
        // {
        //     options.UseNpgsql(builder.Configuration.GetConnectionString("PatriotIndexDb"));
        //     options.UseSnakeCaseNamingConvention();
        //     options.EnableDetailedErrors();
        //     options.EnableSensitiveDataLogging();
        // });

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

        // Register repository classes
        builder.Services.AddScoped<TeamsRepository>();
        builder.Services.AddScoped<GamesRepository>();
        builder.Services.AddScoped<SyncLogRepository>();
        builder.Services.AddScoped<StatsRepository>();

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

        // app.MapGet("/temp", (HttpContext httpContext) => "")
        //     .WithName("TempEndpoint");
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PatriotIndexDbContext>();
            await db.Database.MigrateAsync();
        }

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

        // -----------------------

        await app.RunAsync();
    }
}