using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class LeaderboardQueryRepository(PatriotIndexDbContext_OLD db) : ILeaderboardRepository
{
    private static readonly Dictionary<string, Func<PatriotIndex.Domain.Entities.PlayerSeasonStats, double>> CategorySelectors =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["PASS_YDS"]  = s => s.Stats.Passing?.Yards ?? 0,
            ["PASS_TD"]   = s => s.Stats.Passing?.Touchdowns ?? 0,
            ["PASS_ATT"]  = s => s.Stats.Passing?.Attempts ?? 0,
            ["PASS_CMP"]  = s => s.Stats.Passing?.Completions ?? 0,
            ["PASS_INT"]  = s => s.Stats.Passing?.Interceptions ?? 0,
            ["PASS_RTG"]  = s => (double)(s.Stats.Passing?.Rating ?? 0),
            ["RUSH_YDS"]  = s => s.Stats.Rushing?.Yards ?? 0,
            ["RUSH_TD"]   = s => s.Stats.Rushing?.Touchdowns ?? 0,
            ["RUSH_ATT"]  = s => s.Stats.Rushing?.Attempts ?? 0,
            ["REC_YDS"]   = s => s.Stats.Receiving?.Yards ?? 0,
            ["REC_TD"]    = s => s.Stats.Receiving?.Touchdowns ?? 0,
            ["REC_REC"]   = s => s.Stats.Receiving?.Receptions ?? 0,
            ["REC_TGT"]   = s => s.Stats.Receiving?.Targets ?? 0,
            ["DEF_TKL"]   = s => s.Stats.Defense?.Tackles ?? 0,
            ["DEF_SACK"]  = s => (double)(s.Stats.Defense?.Sacks ?? 0),
            ["DEF_INT"]   = s => s.Stats.Defense?.Interceptions ?? 0,
            ["DEF_FF"]    = s => s.Stats.Defense?.ForcedFumbles ?? 0,
            ["DEF_PD"]    = s => s.Stats.Defense?.PassesDefended ?? 0,
        };

    public async Task<LeaderboardDto> GetLeaderboardAsync(
        string category, int seasonYear, string seasonType,
        PlayerPosition? position, int limit)
    {
        if (!CategorySelectors.TryGetValue(category, out var selector))
            return new LeaderboardDto(category, seasonYear, seasonType, position, []);

        var query = db.PlayerSeasonStats
            .AsNoTracking()
            .Include(s => s.Player!.Team!.Colors)
            .Include(s => s.Player!.Team!.Division!.Conference)
            .Where(s => s.SeasonYear == seasonYear && s.SeasonType == seasonType);

        if (position.HasValue)
            query = query.Where(s => s.Player!.Position == position);

        var stats = await query.ToListAsync();

        var entries = stats
            .Select(s => (stat: s, value: selector(s)))
            .OrderByDescending(x => x.value)
            .Take(limit)
            .Select((x, i) => new LeaderboardEntryDto(
                x.stat.PlayerId,
                x.stat.Player?.Name,
                x.stat.Player?.Position,
                x.stat.Player?.TeamId,
                x.stat.Player?.Team?.Alias,
                x.stat.Player?.Team?.Market,
                x.stat.SeasonYear,
                x.stat.SeasonType,
                x.value))
            .ToList();

        return new LeaderboardDto(category, seasonYear, seasonType, position, entries);
    }
}
