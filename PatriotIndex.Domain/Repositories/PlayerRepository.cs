using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Interfaces;
using PatriotIndex.Domain.Mappings;

namespace PatriotIndex.Domain.Repositories;

public class PlayerRepository(PatriotIndexDbContext db) : IPlayerRepository
{
    public async Task<IReadOnlyList<PlayerSummaryDto>> SearchAsync(
        string? search, Guid? teamId, string? position, string? status)
    {
        var q = db.Players
            .Include(p => p.Team!).ThenInclude(t => t.Division!).ThenInclude(d => d.Conference)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(p => p.Name!.ToLower().Contains(search.ToLower()) ||
                              p.LastName!.ToLower().Contains(search.ToLower()));
        if (teamId.HasValue)
            q = q.Where(p => p.TeamId == teamId);
        if (!string.IsNullOrWhiteSpace(position))
            q = q.Where(p => p.Position == position);
        if (!string.IsNullOrWhiteSpace(status))
            q = q.Where(p => p.Status == status);

        var players = await q.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToListAsync();
        return players.Select(p => p.ToSummary()).ToList();
    }

    public async Task<PlayerDetailDto?> GetByIdAsync(Guid id)
    {
        var player = await db.Players
            .Include(p => p.Team!).ThenInclude(t => t.Division!).ThenInclude(d => d.Conference)
            .Include(p => p.DraftTeam)
            .Include(p => p.SeasonStats)
            .FirstOrDefaultAsync(p => p.Id == id);
        return player?.ToDetail();
    }

    public async Task<IReadOnlyList<PlayerGameLogDto>> GetGameLogAsync(
        Guid playerId, int? seasonYear, string? seasonType)
    {
        var q = db.PlayerGameStats
            .Include(s => s.Game!).ThenInclude(g => g.HomeTeam)
            .Include(s => s.Game!).ThenInclude(g => g.AwayTeam)
            .Where(s => s.PlayerId == playerId);

        if (seasonYear.HasValue)
            q = q.Where(s => s.Game!.SeasonYear == seasonYear);
        if (!string.IsNullOrWhiteSpace(seasonType))
            q = q.Where(s => s.Game!.SeasonType == seasonType);

        var stats = await q.OrderByDescending(s => s.Game!.Scheduled).ToListAsync();

        return stats.Select(s =>
        {
            var game = s.Game!;
            bool isHome = game.HomeTeamId == s.TeamId;
            var opponent = isHome
                ? $"{game.AwayTeam?.Market} {game.AwayTeam?.Name}"
                : $"{game.HomeTeam?.Market} {game.HomeTeam?.Name}";
            return new PlayerGameLogDto(
                game.Id, game.Scheduled, opponent, isHome,
                game.HomePoints, game.AwayPoints,
                s.ToStatBlock());
        }).ToList();
    }
}
