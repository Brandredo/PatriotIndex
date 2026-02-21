using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Interfaces;
using PatriotIndex.Domain.Mappings;

namespace PatriotIndex.Domain.Repositories;

public class TeamRepository(PatriotIndexDbContext db) : ITeamRepository
{
    public async Task<IReadOnlyList<TeamSummaryDto>> GetAllAsync()
    {
        var teams = await db.Teams
            .Include(t => t.Division!).ThenInclude(d => d.Conference)
            .OrderBy(t => t.Market)
            .ToListAsync();
        return teams.Select(t => t.ToSummary()).ToList();
    }

    public async Task<TeamDetailDto?> GetByIdAsync(Guid id)
    {
        var team = await db.Teams
            .Include(t => t.Division!).ThenInclude(d => d.Conference)
            .Include(t => t.Venue)
            .Include(t => t.Coaches)
            .FirstOrDefaultAsync(t => t.Id == id);
        return team?.ToDetail();
    }

    public async Task<IReadOnlyList<PlayerRosterDto>> GetRosterAsync(Guid teamId)
    {
        var players = await db.Players
            .Where(p => p.TeamId == teamId && p.Status == "ACT")
            .OrderBy(p => p.Position).ThenBy(p => p.LastName)
            .ToListAsync();
        return players.Select(p => p.ToRosterDto()).ToList();
    }

    public async Task<StatBlockDto?> GetSeasonStatsAsync(Guid teamId, int seasonYear, string seasonType)
    {
        var stats = await db.TeamSeasonStats
            .FirstOrDefaultAsync(s => s.TeamId == teamId && s.SeasonYear == seasonYear && s.SeasonType == seasonType);
        if (stats == null) return null;

        return new StatBlockDto(
            stats.PassAtt, stats.PassCmp, stats.PassYds, stats.PassTd, stats.PassInt,
            stats.PassRating, stats.PassSacks, stats.PassSackYds,
            stats.RushAtt, stats.RushYds, stats.RushTd, stats.RushAvg, stats.RushLong,
            stats.RecTargets, stats.RecReceptions, stats.RecYds, stats.RecTd, stats.RecAvg, stats.RecLong,
            stats.DefTackles, stats.DefAssists, stats.DefSacks, stats.DefInterceptions,
            stats.DefForcedFumbles, stats.DefPassesDefended, stats.DefQbHits,
            stats.FgAtt, stats.FgMade, stats.FgLong, stats.XpAtt, stats.XpMade,
            stats.PuntAtt, stats.PuntYds, stats.PuntAvg);
    }
}
