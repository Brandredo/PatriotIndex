using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;

namespace PatriotIndex.Ingestion;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        
        // Add a database
        builder.Services.AddDbContextFactory<PatriotIndexDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PatriotIndexDb"));
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
        host.Run();
    }
}