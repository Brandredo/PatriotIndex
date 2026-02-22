using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;

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