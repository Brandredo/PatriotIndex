using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Ingestion.Converters;
using PatriotIndex.Ingestion.Converters.Endpoints;
using PatriotIndex.Ingestion.Converters.Transformers;
using PatriotIndex.Ingestion.Persistence;

namespace PatriotIndex.Ingestion;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Add a database
        builder.Services.AddDbContextFactory<PatriotIndexDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PatriotIndexDb"));
            options.UseSnakeCaseNamingConvention();
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        // Add the HTTP client
        builder.Services.AddHttpClient("MyApi", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["BaseUrl"] ?? throw new ArgumentNullException("Base URL is missing"));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("x-api-key",
                builder.Configuration["ApiKey"] ?? throw new ArgumentNullException("API Key is missing"));
        });

        // Transformers (stateless → Singleton)
        builder.Services.AddSingleton<DriveEventTransformer>();
        builder.Services.AddSingleton<DriveTransformer>();
        builder.Services.AddSingleton<GameTransformer>();
        builder.Services.AddSingleton<PlayerTransformer>();
        builder.Services.AddSingleton<CoachTransformer>();
        builder.Services.AddSingleton<TeamTransformer>();
        builder.Services.AddSingleton<IPeriodTransformer, PeriodTransformer>();
        builder.Services.AddSingleton<DriveAggregatorFactory>();
        
        // Persistence
        builder.Services.AddScoped<GamePbpSaver>();
        builder.Services.AddScoped<TeamsRepository>();

        // Endpoint converters
        builder.Services.AddSingleton<IEndpointDataConverter, PbpDataConverter>();
        builder.Services.AddSingleton<IEndpointDataConverter, TeamProfileDataConverter>();
        builder.Services.AddSingleton<IEndpointDataConverter, BoxscoreDataConverter>();
        builder.Services.AddSingleton<IEndpointDataConverter, ScheduleDataConverter>();

        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();

        // Apply any pending migrations at startup
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PatriotIndexDbContext>();
            await db.Database.MigrateAsync();
        }

        await host.RunAsync();
    }
}
