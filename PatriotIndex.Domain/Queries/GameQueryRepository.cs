using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class GameQueryRepository(PatriotIndexDbContext db) : IGameRepository
{
    public async Task<IReadOnlyList<GameSummaryDto>> GetGamesAsync(
        int? season, string? seasonType, int? week, Guid? teamId)
    {
        var query = db.Games
            .AsNoTracking()
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .AsQueryable();

        if (season.HasValue)
            query = query.Where(g => g.SeasonYear == season);
        if (!string.IsNullOrWhiteSpace(seasonType))
            query = query.Where(g => g.SeasonType == seasonType);
        if (week.HasValue)
            query = query.Where(g => g.WeekSequence == week);
        if (teamId.HasValue)
            query = query.Where(g => g.HomeTeamId == teamId || g.AwayTeamId == teamId);

        var games = await query
            .OrderBy(g => g.Scheduled)
            .Take(200)
            .ToListAsync();

        return games.Select(g => new GameSummaryDto(
            g.Id, g.Scheduled, g.Status, g.SeasonType, g.SeasonYear,
            g.WeekTitle, g.WeekSequence,
            g.HomeTeamId, g.HomeTeam?.Alias, g.HomeTeam?.Market, g.HomeTeam?.Name, g.HomePoints,
            g.AwayTeamId, g.AwayTeam?.Alias, g.AwayTeam?.Market, g.AwayTeam?.Name, g.AwayPoints))
            .ToList();
    }

    public async Task<GamePbpDto?> GetGamePbpAsync(Guid gameId)
    {
        var game = await db.Games
            .AsNoTracking()
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .Include(g => g.Drives)
                .ThenInclude(d => d.OffensiveTeam)
            .Include(g => g.Drives)
                .ThenInclude(d => d.DefensiveTeam)
            .Include(g => g.Drives)
                .ThenInclude(d => d.Plays.OrderBy(p => p.Sequence))
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game is null) return null;

        var drives = game.Drives
            .OrderBy(d => d.Sequence)
            .Select(d =>
            {
                var periodNumber = d.Plays.FirstOrDefault()?.StartSituation != null
                    ? (int?)null
                    : null;

                var plays = d.Plays.Select(p => new PlayDto(
                    p.Id,
                    p.Sequence,
                    p.Clock,
                    p.PlayType,
                    p.Description,
                    p.HomePoints,
                    p.AwayPoints,
                    p.StartSituation.Down,
                    p.StartSituation.YardsToFirstDown,
                    p.StartSituation.PossessionTeamId,
                    game.HomeTeamId == p.StartSituation.PossessionTeamId
                        ? game.HomeTeam?.Alias
                        : game.AwayTeam?.Alias,
                    p.StartSituation.Yardline,
                    p.StartSituation.YardlineTeam,
                    p.EndSituation.Yardline,
                    p.EndSituation.YardlineTeam,
                    p.Statistics.Any(s => s.Touchdowns > 0),
                    p.Statistics.Any(s => s.Interceptions > 0 || s.Fumble > 0 && s.FumbleLost > 0),
                    p.Statistics.Any(s => s.FirstDown > 0),
                    p.Statistics.Any(s => !string.IsNullOrEmpty(s.PenaltyType)),
                    p.FakePunt ?? false,
                    p.FakeFieldGoal ?? false,
                    p.ScreenPass ?? false,
                    p.PlayAction ?? false,
                    p.RunPassOption ?? false,
                    p.HashMark,
                    p.WallClock?.DateTime,
                    p.Statistics.Select(s => new PlayStatDto(
                        Guid.NewGuid(),
                        s.StatType,
                        s.Player.Id == Guid.Empty ? null : s.Player.Id,
                        s.Player.Name,
                        s.Team.Id == Guid.Empty ? null : s.Team.Id,
                        s.Team.Alias,
                        s.Yards,
                        s.Attempt,
                        s.Complete,
                        s.Touchdowns,
                        s.Interceptions,
                        s.Fumble,
                        s.Sack,
                        s.Touchback,
                        null)).ToList()
                )).ToList();

                return new DriveDto(
                    d.Id,
                    periodNumber,
                    d.Sequence,
                    d.StartReason,
                    d.EndReason,
                    d.PlayCount,
                    d.Duration,
                    d.FirstDowns,
                    d.NetYards,
                    d.StartClock,
                    d.EndClock,
                    d.OffensiveTeamId,
                    d.OffensiveTeam?.Alias,
                    d.DefensiveTeamId,
                    d.DefensiveTeam?.Alias,
                    d.OffensivePoints ?? 0,
                    d.DefensivePoints ?? 0,
                    plays);
            }).ToList();

        return new GamePbpDto(
            game.Id, game.Title, game.Status,
            game.HomeTeamId, game.HomeTeam?.Alias, game.HomePoints,
            game.AwayTeamId, game.AwayTeam?.Alias, game.AwayPoints,
            drives);
    }
}
