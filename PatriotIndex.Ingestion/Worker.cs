using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace PatriotIndex.Ingestion;

public class Worker(ILogger<Worker> logger, IHttpClientFactory httpFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var jsonOptions = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        
        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        
        using var client = httpFactory.CreateClient("MyApi");
        
        var response = await client.GetAsync("games/5848514c-3977-4aa3-9db0-94ed5d0ebb34/pbp.json", stoppingToken);
        
        HttpResponseMessage message = response.EnsureSuccessStatusCode();
        
        string content = await message.Content.ReadAsStringAsync(stoppingToken);
        
        using var doc = JsonDocument.Parse(content);
        JsonElement root = doc.RootElement;

        // Navigate by property name
        JsonElement periods = root.GetProperty("periods");

        foreach (JsonElement period in periods.EnumerateArray())
        {
            string periodType = period.GetProperty("period_type").GetString() ?? string.Empty;
            short periodNumber = period.GetProperty("number").GetInt16();
            
            Console.WriteLine($"{periodType} {periodNumber}");
            
            // // Safely try to get a property that may not exist
            // if (period.TryGetProperty("stats", out JsonElement stats))
            // {
            //     int points = stats.GetProperty("points").GetInt32();
            //     Console.WriteLine($"{homeTeam}: {points}");
            // }
        }
        
        logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
        
        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     if (logger.IsEnabled(LogLevel.Information))
        //     {
        //         logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //     }
        //
        //     await Task.Delay(1000, stoppingToken);
        // }
    }
}