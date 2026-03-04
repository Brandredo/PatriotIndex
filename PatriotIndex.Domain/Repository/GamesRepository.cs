using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class GamesRepository(ILogger<GamesRepository> logger, PatriotIndexDbContext ctx)
{
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
        var strategy = ctx.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await ctx.Database.BeginTransactionAsync(ct);

            await UpsertGameAsync(game, ct);
            await UpsertPeriod(game.Periods, ct);

            var allDrives = game.Drives.ToList();
            var allPlays  = allDrives.SelectMany(d => d.Plays).ToList();
            var allEvents = allDrives.SelectMany(d => d.Events).ToList();

            var incomingDriveIds = allDrives.Select(d => d.Id).ToArray();

            await DeleteOrphanedDrives(game.Id, incomingDriveIds, ct);
            await UpsertDrives(allDrives, ct);
            await UpsertPlays(allPlays, ct);
            await UpsertGameEvents(allEvents, ct);

            await tx.CommitAsync(ct);
        });
    }

    private async Task DeleteOrphanedDrives(Guid gameId, Guid[] incomingDriveIds, CancellationToken ct)
    {
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM drives WHERE game_id = {0} AND id != ALL({1})",
            new object[] { gameId, incomingDriveIds }, ct);
    }

    private async Task UpsertGameAsync(Game game, CancellationToken ct)
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
            new object[]
            {
                game.Id, game.SrId, game.Status, game.Scheduled, game.Attendance,
                game.GameType, game.Title, game.Duration,
                game.SeasonYear, game.SeasonType, game.SeasonId, game.WeekSequence, game.WeekTitle,
                game.HomeTeamId, game.AwayTeamId, game.HomePoints, game.AwayPoints, game.VenueId,
                game.WeatherCondition, game.WeatherTemp, game.WeatherHumidity, game.WeatherWindSpeed,
                game.WeatherWindDirection, game.BroadcastNetwork, game.NeutralSite, game.ConferenceGame,
                game.WeekId
            }, ct);
    }

    private async Task UpsertPeriod(IEnumerable<Period> periods, CancellationToken ct)
    {
        var list = periods.ToList();
        if (list.Count == 0) return;

        var sb = new StringBuilder("INSERT INTO periods (id, number, game_id, type, sequence, home_score, away_score) VALUES ");
        var parameters = new List<object>();
        int i = 0;

        foreach (var period in list)
        {
            if (i > 0) sb.Append(", ");
            sb.Append($"({{{i * 7}}}, {{{i * 7 + 1}}}, {{{i * 7 + 2}}}, {{{i * 7 + 3}}}, {{{i * 7 + 4}}}, {{{i * 7 + 5}}}, {{{i * 7 + 6}}})");
            parameters.Add(period.Id);
            parameters.Add(period.Number);
            parameters.Add(period.GameId);
            parameters.Add(period.Type);
            parameters.Add(period.Sequence);
            parameters.Add(period.HomeScore);
            parameters.Add(period.AwayScore);
            i++;
        }

        sb.Append(" ON CONFLICT (id) DO UPDATE SET" +
                  " number = EXCLUDED.number, game_id = EXCLUDED.game_id, type = EXCLUDED.type," +
                  " sequence = EXCLUDED.sequence, home_score = EXCLUDED.home_score, away_score = EXCLUDED.away_score");

        await ctx.Database.ExecuteSqlRawAsync(sb.ToString(), parameters, ct);
    }

    private async Task UpsertDrives(IEnumerable<Drive> drives, CancellationToken ct)
    {
        var list = drives.ToList();
        if (list.Count == 0) return;

        const int cols = 23;
        var sb = new StringBuilder(
            "INSERT INTO drives (" +
            "id, type, sequence, game_id, team_sequence, start_reason, end_reason, play_count, duration, " +
            "first_downs, gained_yards, penalty_yards, net_yards, start_clock, end_clock, " +
            "offensive_team_id, defensive_team_id, offensive_points, defensive_points, " +
            "first_drive_yard_line, last_drive_yard_line, farthest_drive_yard_line, pat_points_attempted) VALUES ");
        var parameters = new List<object>();
        int i = 0;

        foreach (var drive in list)
        {
            if (i > 0) sb.Append(", ");
            sb.Append($"({string.Join(", ", Enumerable.Range(i * cols, cols).Select(n => $"{{{n}}}"))})");
            parameters.Add(drive.Id);
            parameters.Add(drive.Type ?? (object)DBNull.Value);
            parameters.Add(drive.Sequence);
            parameters.Add(drive.GameId);
            parameters.Add(drive.TeamSequence);
            parameters.Add(drive.StartReason);
            parameters.Add(drive.EndReason);
            parameters.Add(drive.PlayCount);
            parameters.Add(drive.Duration);
            parameters.Add(drive.FirstDowns);
            parameters.Add(drive.GainedYards);
            parameters.Add(drive.PenaltyYards);
            parameters.Add(drive.NetYards);
            parameters.Add(drive.StartClock);
            parameters.Add(drive.EndClock);
            parameters.Add(drive.OffensiveTeamId);
            parameters.Add(drive.DefensiveTeamId);
            parameters.Add(drive.OffensivePoints);
            parameters.Add(drive.DefensivePoints);
            parameters.Add(drive.FirstDriveYardLine);
            parameters.Add(drive.LastDriveYardLine);
            parameters.Add(drive.FarthestDriveYardLine);
            parameters.Add(drive.PatPointsAttempted);
            i++;
        }

        sb.Append(
            " ON CONFLICT (id) DO UPDATE SET" +
            " type = EXCLUDED.type, sequence = EXCLUDED.sequence, game_id = EXCLUDED.game_id, team_sequence = EXCLUDED.team_sequence," +
            " start_reason = EXCLUDED.start_reason, end_reason = EXCLUDED.end_reason, play_count = EXCLUDED.play_count," +
            " duration = EXCLUDED.duration, first_downs = EXCLUDED.first_downs, gained_yards = EXCLUDED.gained_yards," +
            " penalty_yards = EXCLUDED.penalty_yards, net_yards = EXCLUDED.net_yards, start_clock = EXCLUDED.start_clock," +
            " end_clock = EXCLUDED.end_clock, offensive_team_id = EXCLUDED.offensive_team_id," +
            " defensive_team_id = EXCLUDED.defensive_team_id, offensive_points = EXCLUDED.offensive_points," +
            " defensive_points = EXCLUDED.defensive_points, first_drive_yard_line = EXCLUDED.first_drive_yard_line," +
            " last_drive_yard_line = EXCLUDED.last_drive_yard_line, farthest_drive_yard_line = EXCLUDED.farthest_drive_yard_line," +
            " pat_points_attempted = EXCLUDED.pat_points_attempted");

        await ctx.Database.ExecuteSqlRawAsync(sb.ToString(), parameters, ct);
    }

    private async Task UpsertPlays(IEnumerable<Play> plays, CancellationToken ct)
    {
        var list = plays.ToList();
        if (list.Count == 0) return;

        // Delete then re-insert: EF Core handles JSONB serialization for statistics and details
        var driveIds = list.Select(p => p.DriveId).Distinct().ToArray();
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM plays WHERE drive_id = ANY({0})",
            new object[] { driveIds }, ct);

        logger.LogInformation("Inserting {Count} plays.", list.Count);
        await ctx.Plays.AddRangeAsync(list, ct);
        await ctx.SaveChangesAsync(ct);
        ctx.ChangeTracker.Clear();
    }

    private async Task UpsertGameEvents(IEnumerable<GameEvent> events, CancellationToken ct)
    {
        var list = events.ToList();
        if (list.Count == 0) return;

        var driveIds = list.Select(e => e.DriveId).Distinct().ToArray();
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM game_events WHERE drive_id = ANY({0})",
            new object[] { driveIds }, ct);

        await ctx.GameEvents.AddRangeAsync(list, ct);
        await ctx.SaveChangesAsync(ct);
        ctx.ChangeTracker.Clear();
    }
}
