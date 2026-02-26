using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Domain.Jobs;

namespace PatriotIndex.Scheduler;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add a database
        builder.Services.AddDbContextFactory<PatriotIndexDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PatriotIndexDb"));
            options.UseSnakeCaseNamingConvention();
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
        
        // Hangfire
        builder.Services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("PatriotIndexDb") ?? throw new ArgumentNullException("Hangfire connection string is missing");
                options.UseNpgsqlConnection(connectionString);
            }, new PostgreSqlStorageOptions
            {
                SchemaName = "hangfire",
                //UseNativeDatabaseTransactions = true,
            }));

        builder.Services.AddHangfireServer(options =>
        {
            options.WorkerCount = 1; // Set the number of workers to 1 to ensure sequential execution
        });
        
        // Register job classes
        builder.Services.AddScoped<TeamProfileJobOrchestrator>();
        
        // Add services to the container.
        builder.Services.AddAuthorization();
    
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseHangfireDashboard("/hangfire");
        
        app.MapGet("/temp", (HttpContext httpContext) => "")
            .WithName("TempEndpoint");
        
        // Hangfire recurring jobs
        RecurringJob.AddOrUpdate<TeamProfileJobOrchestrator>(
            recurringJobId: "tp-orchestrator",
            methodCall: job => job.RunAsync(),
            cronExpression: "*/5 * * * *");
        
        // -----------------------
        
        
        app.Run();
    }
}