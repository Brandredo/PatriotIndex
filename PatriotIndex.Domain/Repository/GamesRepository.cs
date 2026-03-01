using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class GamesRepository(ILogger<GamesRepository> logger, PatriotIndexDbContext ctx)
{
    // public async Task SaveAsync(Game game, CancellationToken ct)
    // {
    //     bool exists = await ctx.Games.AnyAsync(g => g.Id == game.Id, ct);
    //     if (exists)
    //     {
    //         logger.LogWarning("Game {GameId} already exists, skipping.", game.Id);
    //         return;
    //     }
    //     ctx.Games.Add(game);
    //     await ctx.SaveChangesAsync(ct);
    // }
    
    public async Task SaveAsync(IEnumerable<Game> games, CancellationToken ct)
    {
        var gameList = games.ToList();
        if (gameList.Count == 0) return;

        var sb = new StringBuilder("INSERT INTO games (id, status, scheduled) VALUES ");
        var parameters = new List<object>();
        int i = 0;

        foreach (var game in gameList)
        {
            if (i > 0) sb.Append(", ");
            sb.Append($"({{{i * 3}}}, {{{i * 3 + 1}}}, {{{i * 3 + 2}}})");
            parameters.Add(game.Id);
            parameters.Add(game.Status);
            parameters.Add(game.Scheduled);
            i++;
        }

        sb.Append(" ON CONFLICT (id) DO UPDATE SET status = EXCLUDED.status, scheduled = EXCLUDED.scheduled");

        await ctx.Database.ExecuteSqlRawAsync(sb.ToString(), parameters, ct);
    }

    public async Task SaveAsync(Game game, CancellationToken ct)
    {
        // insert the games
        
        
        // insert the periods
        
        
        // insert the drives
        
        
        // insert the drive events
        
        
        
    }

    private async Task UpsertGameAsync(Game game)
    {
        await ctx.Database.ExecuteSqlRawAsync(@"
            INSERT INTO games (
                id, sr_id, status, scheduled, attendance, game_type, title, duration,
                season_year, season_type, season_id, week_sequence, week_title,
                home_team_id, away_team_id, home_points, away_points, venue_id,
                weather_condition, weather_temp, weather_humidity, weather_wind_speed,
                weather_wind_direction, broadcast_network, neutral_site, conference_game, week_id
            ) VALUES (
                {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7},
                {8}, {9}, {10}, {11}, {12},
                {13}, {14}, {15}, {16}, {17},
                {18}, {19}, {20}, {21},
                {22}, {23}, {24}, {25}, {26}
            )
            ON CONFLICT (id) DO UPDATE SET
                sr_id = EXCLUDED.sr_id,
                status = EXCLUDED.status,
                scheduled = EXCLUDED.scheduled,
                attendance = EXCLUDED.attendance,
                game_type = EXCLUDED.game_type,
                title = EXCLUDED.title,
                duration = EXCLUDED.duration,
                season_year = EXCLUDED.season_year,
                season_type = EXCLUDED.season_type,
                season_id = EXCLUDED.season_id,
                week_sequence = EXCLUDED.week_sequence,
                week_title = EXCLUDED.week_title,
                home_team_id = EXCLUDED.home_team_id,
                away_team_id = EXCLUDED.away_team_id,
                home_points = EXCLUDED.home_points,
                away_points = EXCLUDED.away_points,
                venue_id = EXCLUDED.venue_id,
                weather_condition = EXCLUDED.weather_condition,
                weather_temp = EXCLUDED.weather_temp,
                weather_humidity = EXCLUDED.weather_humidity,
                weather_wind_speed = EXCLUDED.weather_wind_speed,
                weather_wind_direction = EXCLUDED.weather_wind_direction,
                broadcast_network = EXCLUDED.broadcast_network,
                neutral_site = EXCLUDED.neutral_site,
                conference_game = EXCLUDED.conference_game,
                week_id = EXCLUDED.week_id",
            game.Id, game.SrId, game.Status, game.Scheduled, game.Attendance,
            game.GameType, game.Title, game.Duration,
            game.SeasonYear, game.SeasonType, game.SeasonId, game.WeekSequence, game.WeekTitle,
            game.HomeTeamId, game.AwayTeamId, game.HomePoints, game.AwayPoints, game.VenueId,
            game.WeatherCondition, game.WeatherTemp, game.WeatherHumidity, game.WeatherWindSpeed,
            game.WeatherWindDirection, game.BroadcastNetwork, game.NeutralSite, game.ConferenceGame,
            game.WeekId);
    }

    private async Task UpsertPeriod(Period period)
    {
        
    }

    private async Task UpsertDrives(IEnumerable<Drive> drives)
    {
        
    }
    
    private async Task UpsertDriveEvents(IEnumerable<DriveEvent> driveEvents)
    {
    }
    
    
    
    
    
}