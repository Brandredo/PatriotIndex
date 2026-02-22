using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Extensions;

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
        
        await ConvertData(content, stoppingToken);
        
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


    private async Task ConvertData(string content, CancellationToken stoppingToken)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync(stoppingToken);

        try
        {
            using var doc = JsonDocument.Parse(content);
            JsonElement root = doc.RootElement;

            var game = new Game
            {
                Id = root.SafeGetProperty("id").GetGuid(),
                Status = root.SafeGetProperty("status").GetString() ?? string.Empty,
                Scheduled = root.SafeGetProperty("scheduled").GetDateTime(),
                Attendance = root.SafeGetProperty("attendance").GetInt32(),
                SrId = root.SafeGetProperty("sr_id").GetString() ?? string.Empty,
                GameType = root.SafeGetProperty("game_type").GetString() ?? string.Empty,
                ConferenceGame = root.SafeGetProperty("conference_game").GetBoolean(),
                Title = root.SafeGetProperty("title").GetString() ?? string.Empty,
                Duration = root.SafeGetProperty("duration").GetString() ?? string.Empty,
                SeasonYear = root.SafeGetProperty("summary").SafeGetProperty("season").SafeGetProperty("year").GetInt32(),
                SeasonType = root.SafeGetProperty("summary").SafeGetProperty("season").SafeGetProperty("type").GetString() ?? string.Empty,
                WeekSequence = root.SafeGetProperty("summary").SafeGetProperty("week").SafeGetProperty("sequence").GetInt32(),// should probably add the weekId too
                VenueId = root.SafeGetProperty("summary").SafeGetProperty("venue").SafeGetProperty("id").GetGuid(),
                HomeTeamId = root.SafeGetProperty("summary").SafeGetProperty("home").SafeGetProperty("id").GetGuid(),
                AwayTeamId = root.SafeGetProperty("summary").SafeGetProperty("away").SafeGetProperty("id").GetGuid(),
                //NeutralSite = root.SafeGetProperty("neutral_site").GetBoolean()
            };
            
            
            // Navigate by property name
            JsonElement periods = root.SafeGetProperty("periods");

            foreach (JsonElement period in periods.EnumerateArray())
            {
                var p = new Period
                {
                    Id = period.SafeGetProperty("id").GetGuid(),
                    GameId = game.Id,
                    Sequence = period.SafeGetProperty("sequence").GetInt32(),
                    Type = period.SafeGetProperty("period_type").GetString() ?? string.Empty,
                    Number = period.SafeGetProperty("number").GetInt32(),
                    HomeScore = period.SafeGetProperty("scoring").SafeGetProperty("home").SafeGetProperty("points").GetInt32(),
                    AwayScore = period.SafeGetProperty("scoring").SafeGetProperty("away").SafeGetProperty("points").GetInt32(),
                };
                
                foreach(var drive in period.SafeGetProperty("pbp").EnumerateArray())
                {

                    if (drive.SafeGetProperty("type").GetString() != "drive")
                    {
                        logger.LogInformation(drive.SafeGetProperty("id").GetString());
                        continue;
                    }
                    
                    var d = new Drive()
                    {
                        Id = drive.SafeGetProperty("id").GetGuid(),
                        PeriodId = p.Id,
                        Sequence = drive.SafeGetProperty("sequence").GetInt32(),
                        StartClock = drive.SafeGetProperty("start_clock").GetString() ?? string.Empty,
                        EndClock = drive.SafeGetProperty("end_clock").GetString() ?? string.Empty,
                        StartReason =  drive.SafeGetProperty("start_reason").GetString() ?? string.Empty,
                        EndReason = drive.SafeGetProperty("end_reason").GetString() ?? string.Empty,
                        PlayCount =  drive.SafeGetProperty("play_count").GetInt32(),
                        Duration = drive.SafeGetProperty("duration").GetString() ?? string.Empty,
                        FirstDowns =  drive.SafeGetProperty("first_downs").GetInt32(),
                        GainedYards = drive.SafeGetProperty("gain").GetInt32(),
                        PenaltyYards =  drive.SafeGetProperty("penalty_yards").GetInt32(),
                        TeamSequence =  drive.SafeGetProperty("team_sequence").GetInt32(),
                        FirstDriveYardLine =  drive.SafeGetProperty("first_drive_yardline").GetInt32(),
                        LastDriveYardLine =   drive.SafeGetProperty("last_drive_yardline").GetInt32(),
                        FarthestDriveYardLine =   drive.SafeGetProperty("farthest_drive_yardline").GetInt32(),
                        NetYards =  drive.SafeGetProperty("net_yards").GetInt32(),
                        PatPointsAttempted = drive.SafeGetProperty("pat_points_attempted").GetInt32(),
                        OffensiveTeamId = drive.SafeGetProperty("offensive_team").SafeGetProperty("id").GetGuid(),
                        DefensiveTeamId = drive.SafeGetProperty("defensive_team").SafeGetProperty("id").GetGuid(),
                    };
                    
                    logger.LogInformation("Processing drive {driveId} with {playCount} plays", d.Id, d.PlayCount);
                    
                    foreach(var play in drive.SafeGetProperty("events").EnumerateArray())
                    {
                        
                        var evt = new DriveEvent
                        {
                            EventType = play.SafeGetProperty("type").ToString(),
                            Id = play.SafeGetProperty("id").GetGuid(),
                            DriveId = d.Id,
                            Sequence = play.SafeGetProperty("sequence").GetInt32(),
                            Clock = play.SafeGetProperty("clock").GetString() ?? string.Empty,
                            HomeScore = play.SafeGetProperty("scoring").SafeGetProperty("home").SafeGetProperty("points").GetInt32(),
                            AwayScore = play.SafeGetProperty("scoring").SafeGetProperty("away").SafeGetProperty("points").GetInt32(),
                            PlayType = play.SafeGetProperty("type").GetString(),
                            WallClock =  play.SafeGetProperty("wall_clock").GetString() ?? string.Empty,
                            Description = play.SafeGetProperty("description").GetString() ?? string.Empty,
                            PassRoute = play.SafeGetProperty("pass_route").GetString() ?? string.Empty,
                            QbSnap = play.SafeGetProperty("qb_snap").GetString() ?? string.Empty,
                            Huddle = play.SafeGetProperty("huddle").GetString() ?? string.Empty,
                            MenInBox = play.SafeGetProperty("men_in_box").GetInt32(),
                            LeftTightEnds = play.SafeGetProperty("left_tight_ends").GetInt32(),
                            RightTightEnds = play.SafeGetProperty("right_tight_ends").GetInt32(),
                            HashMark = play.SafeGetProperty("hash_mark").GetString() ?? string.Empty,
                            PlayersRushed = play.SafeGetProperty("players_rushed").GetInt32(),
                            PlayDirection = play.SafeGetProperty("play_direction").GetString() ?? string.Empty,
                            PocketLocation = play.SafeGetProperty("pocket_location").GetString() ?? string.Empty,
                            FakePunt = play.SafeGetProperty("fake_punt").GetBoolean(),
                            FakeFieldGoal = play.SafeGetProperty("fake_field_goal").GetBoolean(),
                            ScreenPass = play.SafeGetProperty("screen_pass").GetBoolean(),
                            Blitz = play.SafeGetProperty("blitz").GetBoolean(),
                            PlayAction = play.SafeGetProperty("play_action").GetBoolean(),
                            RunPassOption = play.SafeGetProperty("run_pass_option").GetBoolean(),
                            StartClock = play.SafeGetProperty("start_clock").GetString() ?? string.Empty,
                            StartDown = play.SafeGetProperty("start_down").GetInt32(),
                            StartYardsToGain = play.SafeGetProperty("start_yards_to_gain").GetInt32(),
                            StartLocationYardLine = play.SafeGetProperty("start_location_yardline").GetInt32(),
                            StartPossessionTeamId = play.SafeGetProperty("start_possession_team").SafeGetProperty("id").GetGuid(),
                            EndClock = play.SafeGetProperty("end_clock").GetString() ?? string.Empty,
                            EndDown = play.SafeGetProperty("end_down").GetInt32(),
                            EndYardsToGain = play.SafeGetProperty("end_yards_to_gain").GetInt32(),
                            EndLocationYardLine = play.SafeGetProperty("end_location_yardline").GetInt32(),
                            EndPossessionTeamId = play.SafeGetProperty("end_possession_team").SafeGetProperty("id").GetGuid(),
                        };
                        
                        d.Plays.Add(evt);
                    }
                    
                    p.Drives.Add(d);
                }
                
                game.Periods.Add(p);
            }
            
            
            ctx.Games.Add(game);
            await ctx.SaveChangesAsync(stoppingToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        
        
        
    }
    
    
    
    
    
}