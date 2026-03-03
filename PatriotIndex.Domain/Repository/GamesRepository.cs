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
        var strategy = ctx.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await ctx.Database.BeginTransactionAsync(ct);

            await UpsertGameAsync(game, ct);
            await UpsertPeriod(game.Periods, ct);

            var allDrives = game.Drives.ToList();
            var allPlays  = allDrives.SelectMany(d => d.Plays).ToList();

            var incomingDriveIds = allDrives.Select(d => d.Id).ToArray();
            var incomingPlayIds  = allPlays.Select(p => p.Id).ToArray();

            await DeleteOrphanedDriveEvents(incomingDriveIds, incomingPlayIds, ct);
            await DeleteOrphanedDrives(game.Id, incomingDriveIds, ct);

            await UpsertDrives(allDrives, ct);
            await UpsertDriveEvents(allPlays, ct);
            await UpsertPlayStatistics(allPlays, ct);

            await tx.CommitAsync(ct);
        });
    }

    private async Task DeleteOrphanedDrives(Guid gameId, Guid[] incomingDriveIds, CancellationToken ct)
    {
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM drives WHERE game_id = {0} AND id != ALL({1})",
            new object[] { gameId, incomingDriveIds }, ct);
    }

    private async Task DeleteOrphanedDriveEvents(Guid[] incomingDriveIds, Guid[] incomingPlayIds, CancellationToken ct)
    {
        if (incomingDriveIds.Length == 0) return;
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM pbp_drive_events WHERE drive_id = ANY({0}) AND id != ALL({1})",
            new object[] { incomingDriveIds, incomingPlayIds }, ct);
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

    private async Task UpsertDriveEvents(IEnumerable<DriveEvent> driveEvents, CancellationToken ct)
    {
        var list = driveEvents.ToList();
        if (list.Count == 0) return;

        const int cols = 37;
        var sb = new StringBuilder(
            "INSERT INTO pbp_drive_events (" +
            "id, drive_id, period_id, event_type, sequence, clock, wall_clock, description, " +
            "home_score, away_score, play_type, pass_route, qb_snap, huddle, men_in_box, " +
            "left_tight_ends, right_tight_ends, hash_mark, players_rushed, play_direction, pocket_location, " +
            "fake_punt, fake_field_goal, screen_pass, blitz, play_action, run_pass_option, " +
            "start_clock, start_down, start_yards_to_gain, start_location_yard_line, start_possession_team_id, " +
            "end_clock, end_down, end_yards_to_gain, end_location_yard_line, end_possession_team_id) VALUES ");
        var parameters = new List<object>();
        int i = 0;

        foreach (var evt in list)
        {
            if (i > 0) sb.Append(", ");
            sb.Append($"({string.Join(", ", Enumerable.Range(i * cols, cols).Select(n => $"{{{n}}}"))})");
            parameters.Add(evt.Id);
            parameters.Add(evt.DriveId);
            parameters.Add(evt.PeriodId);
            parameters.Add(evt.EventType);
            parameters.Add(evt.Sequence);
            parameters.Add(evt.Clock);
            parameters.Add(evt.WallClock);
            parameters.Add(evt.Description);
            parameters.Add(evt.HomeScore);
            parameters.Add(evt.AwayScore);
            parameters.Add(evt.PlayType);
            parameters.Add(evt.PassRoute);
            parameters.Add(evt.QbSnap);
            parameters.Add(evt.Huddle);
            parameters.Add(evt.MenInBox);
            parameters.Add(evt.LeftTightEnds);
            parameters.Add(evt.RightTightEnds);
            parameters.Add(evt.HashMark);
            parameters.Add(evt.PlayersRushed);
            parameters.Add(evt.PlayDirection);
            parameters.Add(evt.PocketLocation);
            parameters.Add(evt.FakePunt);
            parameters.Add(evt.FakeFieldGoal);
            parameters.Add(evt.ScreenPass);
            parameters.Add(evt.Blitz);
            parameters.Add(evt.PlayAction);
            parameters.Add(evt.RunPassOption);
            parameters.Add(evt.StartClock);
            parameters.Add(evt.StartDown);
            parameters.Add(evt.StartYardsToGain);
            parameters.Add(evt.StartLocationYardLine);
            parameters.Add(evt.StartPossessionTeamId);
            parameters.Add(evt.EndClock);
            parameters.Add(evt.EndDown);
            parameters.Add(evt.EndYardsToGain);
            parameters.Add(evt.EndLocationYardLine);
            parameters.Add(evt.EndPossessionTeamId);
            i++;
        }

        sb.Append(
            " ON CONFLICT (id) DO UPDATE SET" +
            " drive_id = EXCLUDED.drive_id, period_id = EXCLUDED.period_id, event_type = EXCLUDED.event_type," +
            " sequence = EXCLUDED.sequence, clock = EXCLUDED.clock, wall_clock = EXCLUDED.wall_clock," +
            " description = EXCLUDED.description, home_score = EXCLUDED.home_score, away_score = EXCLUDED.away_score," +
            " play_type = EXCLUDED.play_type, pass_route = EXCLUDED.pass_route, qb_snap = EXCLUDED.qb_snap," +
            " huddle = EXCLUDED.huddle, men_in_box = EXCLUDED.men_in_box, left_tight_ends = EXCLUDED.left_tight_ends," +
            " right_tight_ends = EXCLUDED.right_tight_ends, hash_mark = EXCLUDED.hash_mark," +
            " players_rushed = EXCLUDED.players_rushed, play_direction = EXCLUDED.play_direction," +
            " pocket_location = EXCLUDED.pocket_location, fake_punt = EXCLUDED.fake_punt," +
            " fake_field_goal = EXCLUDED.fake_field_goal, screen_pass = EXCLUDED.screen_pass," +
            " blitz = EXCLUDED.blitz, play_action = EXCLUDED.play_action, run_pass_option = EXCLUDED.run_pass_option," +
            " start_clock = EXCLUDED.start_clock, start_down = EXCLUDED.start_down," +
            " start_yards_to_gain = EXCLUDED.start_yards_to_gain, start_location_yard_line = EXCLUDED.start_location_yard_line," +
            " start_possession_team_id = EXCLUDED.start_possession_team_id, end_clock = EXCLUDED.end_clock," +
            " end_down = EXCLUDED.end_down, end_yards_to_gain = EXCLUDED.end_yards_to_gain," +
            " end_location_yard_line = EXCLUDED.end_location_yard_line, end_possession_team_id = EXCLUDED.end_possession_team_id");

        await ctx.Database.ExecuteSqlRawAsync(sb.ToString(), parameters, ct);
    }

    private async Task UpsertPlayStatistics(IEnumerable<DriveEvent> driveEvents, CancellationToken ct)
    {
        var plays = driveEvents.ToList();
        if (plays.Count == 0) return;

        var allStats = plays
            .SelectMany(p => p.PlayStats)
            .Where(s => s is not UnknownPlayStat)
            .ToList();

        var unknownCount = plays.SelectMany(p => p.PlayStats).Count(s => s is UnknownPlayStat);
        if (unknownCount > 0)
            logger.LogWarning("Skipping {Count} UnknownPlayStat entries (no EF mapping).", unknownCount);

        if (allStats.Count == 0) return;

        // Delete existing rows so re-saves are idempotent
        var playIds = plays.Select(p => p.Id).ToArray();
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM play_statistics WHERE play_id = ANY({0})",
            new object[] { playIds }, ct);

        logger.LogInformation("Inserting {Count} play statistics.", allStats.Count);
        await ctx.PlayStatistics.AddRangeAsync(allStats, ct);
        await ctx.SaveChangesAsync(ct);
        ctx.ChangeTracker.Clear();
    }
}