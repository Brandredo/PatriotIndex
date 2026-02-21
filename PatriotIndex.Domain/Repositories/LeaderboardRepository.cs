using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Repositories;

public class LeaderboardRepository(PatriotIndexDbContext db) : ILeaderboardRepository
{
    private static readonly HashSet<string> ValidCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "pass_yds", "pass_td", "pass_att", "pass_cmp", "pass_int", "pass_rating",
        "rush_yds", "rush_td", "rush_att",
        "rec_yds", "rec_td", "rec_receptions", "rec_targets",
        "def_tackles", "def_sacks", "def_interceptions", "def_forced_fumbles",
        "fg_made", "punt_yds",
    };

    public async Task<LeaderboardDto> GetLeaderboardAsync(
        string category, int seasonYear, string seasonType, string? position, int limit)
    {
        if (!ValidCategories.Contains(category))
            throw new ArgumentException($"Unknown leaderboard category: {category}");

        var q = db.PlayerSeasonStats
            .Include(s => s.Player!).ThenInclude(p => p.Team)
            .Where(s => s.SeasonYear == seasonYear && s.SeasonType == seasonType);

        if (!string.IsNullOrWhiteSpace(position))
            q = q.Where(s => s.Player!.Position == position);

        var all = await q.ToListAsync();

        Func<PlayerSeasonStats, double> selector = category.ToLower() switch
        {
            "pass_yds" => s => s.PassYds,
            "pass_td" => s => s.PassTd,
            "pass_att" => s => s.PassAtt,
            "pass_cmp" => s => s.PassCmp,
            "pass_int" => s => s.PassInt,
            "pass_rating" => s => s.PassRating,
            "rush_yds" => s => s.RushYds,
            "rush_td" => s => s.RushTd,
            "rush_att" => s => s.RushAtt,
            "rec_yds" => s => s.RecYds,
            "rec_td" => s => s.RecTd,
            "rec_receptions" => s => s.RecReceptions,
            "rec_targets" => s => s.RecTargets,
            "def_tackles" => s => s.DefTackles,
            "def_sacks" => s => s.DefSacks,
            "def_interceptions" => s => s.DefInterceptions,
            "def_forced_fumbles" => s => s.DefForcedFumbles,
            "fg_made" => s => s.FgMade,
            "punt_yds" => s => s.PuntYds,
            _ => throw new ArgumentException("Invalid category"),
        };

        var leaders = all
            .OrderByDescending(selector)
            .Take(limit)
            .Select(s => new LeaderboardEntryDto(
                s.PlayerId,
                s.Player?.Name,
                s.Player?.Position,
                s.TeamId,
                s.Player?.Team?.Alias,
                s.Player?.Team?.Market,
                s.SeasonYear,
                s.SeasonType,
                selector(s)))
            .ToList();

        return new LeaderboardDto(category, seasonYear, seasonType, position, leaders);
    }
}
