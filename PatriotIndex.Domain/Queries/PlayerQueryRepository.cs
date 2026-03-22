using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class PlayerQueryRepository(PatriotIndexDbContext_OLD db, ICacheService cache) : IPlayerRepository
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
            p.Id, p.Name, p.FirstName, p.LastName, p.Jersey, p.Position.ToString(),
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
                MapStatBlock(gs.Stats));
        }).ToList();
    }

    internal static StatBlockDto MapStatBlock(PlayerGameStatsBlock s) => new(
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
        (double)(s.Rushing?.AvgYards ?? 0),
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

    // ── Stats page ────────────────────────────────────────────────────────

    public async Task<PagedResultDto<PlayerSeasonStats>> GetAllPlayersStatsAsync(
        string? positionGroup, int season, string seasonType,
        string? cursor, int limit, CancellationToken ct = default)
    {
        var positions = ParsePositionGroup(positionGroup);

        int? cursorKey = null;
        Guid? cursorPlayerId = null;
        if (cursor is not null)
        {
            var parts = cursor.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[0], out var k) && Guid.TryParse(parts[1], out var pid))
            {
                cursorKey = k;
                cursorPlayerId = pid;
            }
        }

        var baseQuery = db.PlayerSeasonStats
            .AsNoTracking()
            .Include(s => s.Player!.Team!.Colors)
            .Where(s =>
                s.SeasonYear == season &&
                s.SeasonType == seasonType &&
                s.SortKey != null &&
                s.Player != null &&
                s.Player.Position != null &&
                (positions.Length == 0 || positions.Contains(s.Player.Position!.Value)));

        if (cursorKey.HasValue && cursorPlayerId.HasValue)
        {
            var ck = cursorKey.Value;
            var cp = cursorPlayerId.Value;
            baseQuery = baseQuery.Where(s =>
                s.SortKey < ck || (s.SortKey == ck && s.PlayerId > cp));
        }

        var items = await baseQuery
            .OrderByDescending(s => s.SortKey)
            .ThenBy(s => s.PlayerId)
            .Take(limit + 1)
            .ToListAsync(ct);

        var hasNextPage = items.Count > limit;
        if (hasNextPage) items.RemoveAt(items.Count - 1);

        var nextCursor = hasNextPage
            ? $"{items[^1].SortKey}:{items[^1].PlayerId}"
            : null;

        return new PagedResultDto<PlayerSeasonStats>(items.ToArray(), nextCursor, items.Count);
    }

    private async Task<List<PlayerSeasonStats>> FetchAllPlayersFromDb(
        string? positionGroup, int season, string seasonType)
    {
        var positions = ParsePositionGroup(positionGroup);

        var query = db.PlayerSeasonStats
            .AsNoTracking()
            .Include(s => s.Player!.Team!.Colors)
            .Where(s => s.SeasonYear == season && s.SeasonType == seasonType
                        && s.Player != null && s.GamesPlayed > 0);

        if (positions.Length > 0)
            query = query.Where(s => s.Player!.Position != null && positions.Contains(s.Player.Position!.Value));

        var rows = await query.ToListAsync();

        return rows;
        // .Select(s => MapPlayerSummary(s, positionGroup))
        // .OrderByDescending(x => DefaultSortValue(x, positionGroup))
        //.ToArray();
    }

    private static PlayerPosition[] ParsePositionGroup(string? group) => group?.ToUpper() switch
    {
        "QB"    => [PlayerPosition.QB],
        "RB"    => [PlayerPosition.RB, PlayerPosition.FB],
        "WR_TE" => [PlayerPosition.WR, PlayerPosition.TE],
        "DEF"   => [PlayerPosition.DL, PlayerPosition.DE, PlayerPosition.DT,
                    PlayerPosition.LB, PlayerPosition.MLB, PlayerPosition.OLB,
                    PlayerPosition.CB, PlayerPosition.FS, PlayerPosition.SS,
                    PlayerPosition.SAF, PlayerPosition.DB],
        "ST"    => [PlayerPosition.K, PlayerPosition.P, PlayerPosition.LS],
        _       => []
    };

    // private static PlayerStatsSummaryDto MapPlayerSummary(PlayerSeasonStats s, string? group)
    // {
    //     var p   = s.Player!;
    //     var st  = s.Stats;
    //     var pas = st.Passing;
    //     var rus = st.Rushing;
    //     var rec = st.Receiving;
    //     var def = st.Defense;
    //     var fg  = st.FieldGoals;
    //     var xp  = st.ExtraPoints;
    //     var pun = st.Punts;
    //     int gp  = Math.Max(s.GamesPlayed, 1);
    //
    //     
    //
    //     return new PlayerStatsSummaryDto(
    //         p.Id,
    //         p.Name,
    //         p.Position?.ToString(),
    //         p.TeamId,
    //         p.Team?.Alias,
    //         p.Team?.Market,
    //         s.GamesPlayed,
    //         s.GamesStarted,
    //         // Passing core
    //         pas?.Attempts,
    //         pas?.Completions,
    //         pas is not null ? Pct(pas.Completions, pas.Attempts) : null,
    //         pas?.Yards,
    //         pas is not null ? SafeDivInt(pas.Yards, gp) : null,
    //         pas?.Touchdowns,
    //         pas?.Interceptions,
    //         pas is not null ? (decimal?)pas.Rating : null,
    //         pas?.Sacks,
    //         // Passing advanced
    //         pas?.AirYards,
    //         pas is not null ? SafeDivInt(pas.AirYards, pas.Attempts) : null,
    //         pas?.AvgPocketTime,
    //         pas is not null ? Pct(pas.PoorThrows, pas.Attempts) : null,
    //         pas?.Blitzes,
    //         pas?.Hurries,
    //         // Rushing core
    //         rus?.Attempts,
    //         rus?.Yards,
    //         rus is not null ? SafeDiv((decimal?)rus.AvgYards, 1) : null,
    //         rus?.Touchdowns,
    //         // Rushing advanced
    //         rus?.BrokenTackles,
    //         rus is not null ? Pct(rus.BrokenTackles, rus.Attempts) : null,
    //         rus?.YardsAfterContact,
    //         rus is not null ? SafeDivInt(rus.YardsAfterContact, rus.Attempts) : null,
    //         // Receiving core
    //         rec?.Targets,
    //         rec?.Receptions,
    //         rec?.Yards,
    //         rec is not null ? (decimal?)rec.AvgYards : null,
    //         rec?.Touchdowns,
    //         rec is not null ? Pct(rec.Receptions, rec.Targets) : null,
    //         // Receiving advanced
    //         rec?.YardsAfterCatch,
    //         rec is not null ? SafeDivInt(rec.YardsAfterCatch, rec.Receptions) : null,
    //         rec?.AirYards,
    //         rec?.DroppedPasses,
    //         rec is not null ? Pct(rec.DroppedPasses, rec.Targets) : null,
    //         // Defense core
    //         def?.Tackles,
    //         def?.Assists,
    //         def is not null ? (decimal?)def.Sacks : null,
    //         def?.Interceptions,
    //         def?.PassesDefended,
    //         def?.ForcedFumbles,
    //         def?.QbHits,
    //         // Defense advanced
    //         def?.MissedTackles,
    //         def?.Blitzes,
    //         def?.Hurries,
    //         def?.Knockdowns,
    //         // Kicker
    //         fg?.Made,
    //         fg?.Attempts,
    //         fg?.Pct,
    //         fg?.Longest,
    //         fg?.Made19, fg?.Attempts19,
    //         fg?.Made29, fg?.Attempts29,
    //         fg?.Made39, fg?.Attempts39,
    //         fg?.Made49, fg?.Attempts49,
    //         fg?.Made50, fg?.Attempts50,
    //         xp?.Made,
    //         xp?.Attempts,
    //         // Punter
    //         pun?.Attempts,
    //         pun?.AvgYards,
    //         pun?.AvgNetYards,
    //         pun?.Inside20,
    //         pun?.AvgHangTime
    //     );
    // }

    private static PagedResultDto<T> Paginate<T>(
        T[] all, string? cursor, int limit, Func<T, string> getId)
    {
        var start = cursor is null ? 0
            : Array.FindIndex(all, x => getId(x) == cursor) + 1;
        if (start < 0) start = 0;
        var page = all.Skip(start).Take(limit).ToArray();
        var nextCursor = page.Length == limit && start + limit < all.Length
            ? getId(page.Last()) : null;
        return new PagedResultDto<T>(page, nextCursor, all.Length);
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
