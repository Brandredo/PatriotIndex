using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class PlayerQueryRepository(PatriotIndexDbContext db) : IPlayerRepository
{
    public async Task<IReadOnlyList<PlayerSummaryDto>> SearchAsync(
        string? search, Guid? teamId, PlayerPosition? position, string? status)
    {
        var query = db.Players
            .AsNoTracking()
            .Include(p => p.Team!.Colors)
            .Include(p => p.Team!.Division!.Conference)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower()));
        if (teamId.HasValue)
            query = query.Where(p => p.TeamId == teamId);
        if (position.HasValue)
            query = query.Where(p => p.Position == position);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p => p.Status == status);

        var players = await query.OrderBy(p => p.Name).Take(100).ToListAsync();
        return players.Select(p => new PlayerSummaryDto(
            p.Id, p.Name, p.FirstName, p.LastName, p.Jersey, p.Position,
            p.Status,
            p.Team is null ? null : new TeamSummaryDto(
                p.Team.Id, p.Team.Name, p.Team.Market, p.Team.Alias,
                p.Team.Colors,
                p.Team.Division is null ? null : new DivisionSummaryDto(
                    p.Team.Division.Id, p.Team.Division.Name, p.Team.Division.Alias,
                    new ConferenceSummaryDto(
                        p.Team.Division.Conference!.Id,
                        p.Team.Division.Conference.Name,
                        p.Team.Division.Conference.Alias))))).ToList();
    }

    public async Task<PlayerDetailDto?> GetByIdAsync(Guid id)
    {
        var p = await db.Players
            .AsNoTracking()
            .Include(p => p.Team!.Colors)
            .Include(p => p.Team!.Division!.Conference)
            .Include(p => p.DraftTeam)
            .Include(p => p.SeasonStats).ThenInclude(playerSeasonStats => playerSeasonStats.Team)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (p is null) return null;

        var seasonStats = (p.SeasonStats ?? [])
            .OrderByDescending(s => s.SeasonYear)
            .ThenBy(s => s.SeasonType)
            .Select(s => new PlayerSeasonStatsDto(
                s.Id, s.SeasonYear, s.SeasonType, s.GamesPlayed, s.GamesStarted, s?.Team?.Name ?? "NoTeam",
                MapStatBlock(s.Stats)))
            .ToList();

        return new PlayerDetailDto(
            p.Id, p.Name, p.FirstName, p.LastName, p.Jersey, p.Position,
            p.Status, p.Experience, p.Height, (int?)p.Weight,
            p.BirthDate?.ToString("yyyy-MM-dd"),
            p.College, p.RookieYear, p.Salary, p.SrId,
            p.DraftYear, p.DraftRound, p.DraftPick,
            p.DraftTeam?.Alias,
            p.Team is null ? null : new TeamSummaryDto(
                p.Team.Id, p.Team.Name, p.Team.Market, p.Team.Alias,
                p.Team.Colors,
                p.Team.Division is null ? null : new DivisionSummaryDto(
                    p.Team.Division.Id, p.Team.Division.Name, p.Team.Division.Alias,
                    new ConferenceSummaryDto(
                        p.Team.Division.Conference!.Id,
                        p.Team.Division.Conference.Name,
                        p.Team.Division.Conference.Alias))),
            seasonStats);
    }

    public async Task<IReadOnlyList<PlayerGameLogDto>> GetGameLogAsync(
        Guid playerId, int? seasonYear, string? seasonType)
    {
        var query = db.PlayerGameStats
            .AsNoTracking()
            .Include(gs => gs.Game!.HomeTeam)
            .Include(gs => gs.Game!.AwayTeam)
            .Include(gs => gs.Team)
            .Where(gs => gs.PlayerId == playerId);

        if (seasonYear.HasValue)
            query = query.Where(gs => gs.Game!.SeasonYear == seasonYear);
        if (!string.IsNullOrWhiteSpace(seasonType))
            query = query.Where(gs => gs.Game!.SeasonType == seasonType);

        var results = await query
            .OrderByDescending(gs => gs.Game!.Scheduled)
            .Take(50)
            .ToListAsync();

        return results.Select(gs =>
        {
            var g = gs.Game!;
            var isHome = g.HomeTeamId == gs.TeamId;
            var opponent = isHome
                ? g.AwayTeam?.Alias ?? "?"
                : g.HomeTeam?.Alias ?? "?";

            return new PlayerGameLogDto(
                g.Id, g.Scheduled, opponent, isHome, g.HomePoints, g.AwayPoints,
                new StatBlockDto(
                    gs.PassAtt, gs.PassCmp, gs.PassYds, gs.PassTd, gs.PassInt,
                    gs.PassRating, gs.PassSacks, gs.PassSackYds,
                    gs.RushAtt, gs.RushYds, gs.RushTd, gs.RushAvg, gs.RushLong,
                    gs.RecTargets, gs.RecReceptions, gs.RecYds, gs.RecTd, gs.RecAvg, gs.RecLong,
                    gs.DefTackles, gs.DefAssists, gs.DefSacks, gs.DefInterceptions,
                    gs.DefForcedFumbles, gs.DefPassesDefended, gs.DefQbHits,
                    gs.FgAtt, gs.FgMade, gs.FgLong, gs.XpAtt, gs.XpMade,
                    gs.PuntAtt, gs.PuntYds, gs.PuntAvg));
        }).ToList();
    }

    internal static StatBlockDto MapStatBlock(PlayerStatBlock s) => new(
        s.Passing?.Attempts ?? 0,
        s.Passing?.Completions ?? 0,
        s.Passing?.Yards ?? 0,
        s.Passing?.Touchdowns ?? 0,
        s.Passing?.Interceptions ?? 0,
        (double)(s.Passing?.Rating ?? 0),
        s.Passing?.Sacks ?? 0,
        s.Passing?.SackYards ?? 0,
        s.Rushing?.Attempts ?? 0,
        s.Rushing?.Yards ?? 0,
        s.Rushing?.Touchdowns ?? 0,
        s.Rushing?.AvgYards ?? 0,
        s.Rushing?.Longest ?? 0,
        s.Receiving?.Targets ?? 0,
        s.Receiving?.Receptions ?? 0,
        s.Receiving?.Yards ?? 0,
        s.Receiving?.Touchdowns ?? 0,
        (double)(s.Receiving?.AvgYards ?? 0),
        s.Receiving?.Longest ?? 0,
        s.Defense?.Tackles ?? 0,
        s.Defense?.Assists ?? 0,
        (double)(s.Defense?.Sacks ?? 0),
        s.Defense?.Interceptions ?? 0,
        s.Defense?.ForcedFumbles ?? 0,
        s.Defense?.PassesDefended ?? 0,
        s.Defense?.QbHits ?? 0,
        s.FieldGoals?.Attempts ?? 0,
        s.FieldGoals?.Made ?? 0,
        s.FieldGoals?.Longest ?? 0,
        s.ExtraPoints?.Attempts ?? 0,
        s.ExtraPoints?.Made ?? 0,
        s.Punts?.Attempts ?? 0,
        s.Punts?.Yards ?? 0,
        (double)(s.Punts?.AvgYards ?? 0));
}
