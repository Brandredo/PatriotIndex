using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Interfaces;
using PatriotIndex.Domain.Mappings;

namespace PatriotIndex.Domain.Repositories;

public class PlayByPlayRepository(PatriotIndexDbContext db) : IPlayByPlayRepository
{
    public async Task<GamePbpDto?> GetGamePbpAsync(Guid gameId)
    {
        var game = await db.Games
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null) return null;

        var drives = await db.Drives
            .Include(d => d.Plays).ThenInclude(p => p.PlayStats)
            .Where(d => d.GameId == gameId)
            .OrderBy(d => d.PeriodNumber).ThenBy(d => d.Sequence)
            .ToListAsync();

        // Build team alias lookup
        var teamIds = drives
            .SelectMany(d => new[] { d.OffensiveTeamId, d.DefensiveTeamId })
            .Where(id => id.HasValue).Select(id => id!.Value)
            .Distinct().ToList();

        var playerIds = drives
            .SelectMany(d => d.Plays)
            .SelectMany(p => p.PlayStats)
            .Where(s => s.PlayerId.HasValue)
            .Select(s => s.PlayerId!.Value)
            .Distinct().ToList();

        var teamAliases = await db.Teams
            .Where(t => teamIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Alias);

        var playerNames = await db.Players
            .Where(p => playerIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.Name ?? $"{p.FirstName} {p.LastName}");

        return new GamePbpDto(
            game.Id, game.Title, game.Status,
            game.HomeTeamId, game.HomeTeam?.Alias, game.HomePoints,
            game.AwayTeamId, game.AwayTeam?.Alias, game.AwayPoints,
            drives.Select(d => d.ToDto(teamAliases, playerNames)).ToList());
    }

    public async Task<DriveDto?> GetDriveAsync(Guid driveId)
    {
        var drive = await db.Drives
            .Include(d => d.Plays).ThenInclude(p => p.PlayStats)
            .FirstOrDefaultAsync(d => d.Id == driveId);
        if (drive == null) return null;

        var teamIds = new[] { drive.OffensiveTeamId, drive.DefensiveTeamId }
            .Where(id => id.HasValue).Select(id => id!.Value).ToList();

        var playerIds = drive.Plays
            .SelectMany(p => p.PlayStats)
            .Where(s => s.PlayerId.HasValue)
            .Select(s => s.PlayerId!.Value).Distinct().ToList();

        var teamAliases = await db.Teams
            .Where(t => teamIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Alias);

        var playerNames = await db.Players
            .Where(p => playerIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.Name ?? $"{p.FirstName} {p.LastName}");

        return drive.ToDto(teamAliases, playerNames);
    }
}
