using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion;

public class Worker(ILogger<Worker> logger, IHttpClientFactory httpFactory, IDbContextFactory<PatriotIndexDbContext> dbContextFactory) : BackgroundService
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


    private void ConvertData(string content)
    {
        
        using var ctx = dbContextFactory.CreateDbContext();
        
        using var doc = JsonDocument.Parse(content);
        JsonElement root = doc.RootElement;

        var game = new Game
        {
            Id = root.GetProperty("id").GetGuid(),
            Status = root.GetProperty("status").GetString() ?? string.Empty,
            Scheduled = root.GetProperty("scheduled").GetDateTime(),
            Attendance = root.GetProperty("attendance").GetInt32(),
            SrId = root.GetProperty("sr_id").GetString() ?? string.Empty,
            GameType = root.GetProperty("game_type").GetString() ?? string.Empty,
            ConferenceGame = root.GetProperty("conference_game").GetBoolean(),
            Title = root.GetProperty("title").GetString() ?? string.Empty,
            Duration = root.GetProperty("duration").GetString() ?? string.Empty,
            SeasonYear = root.GetProperty("summary").GetProperty("season").GetProperty("season_year").GetInt32(),
            SeasonType = root.GetProperty("summary").GetProperty("season").GetProperty("season_type").GetString() ?? string.Empty,
            WeekSequence = root.GetProperty("summary").GetProperty("week").GetProperty("sequence").GetInt32(),// should probably add the weekId too
            VenueId = root.GetProperty("summary").GetProperty("venue").GetProperty("id").GetGuid(),
            HomeTeamId = root.GetProperty("summary").GetProperty("home").GetProperty("id").GetGuid(),
            AwayTeamId = root.GetProperty("summary").GetProperty("away").GetProperty("id").GetGuid(),
            //NeutralSite = root.GetProperty("neutral_site").GetBoolean()
        };
        
        
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
        
        
    }
    
    
}