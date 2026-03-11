using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class TeamQueryRepository(PatriotIndexDbContext db) : ITeamRepository
{
    public async Task<IReadOnlyList<TeamSummaryDto>> GetAllAsync()
    {
        var teams = await db.Teams
            .AsNoTracking()
            .Include(t => t.Colors)
            .Include(t => t.Division!.Conference)
            .Where(t => t.IsActive)
            .OrderBy(t => t.Market)
            .ToListAsync();

        return teams.Select(ToSummary).ToList();
    }

    public async Task<TeamDetailDto?> GetByIdAsync(Guid id)
    {
        var t = await db.Teams
            .AsNoTracking()
            .Include(t => t.Colors)
            .Include(t => t.Division!.Conference)
            .Include(t => t.Venue)
            .Include(t => t.Coaches)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (t is null) return null;

        return new TeamDetailDto(
            t.Id, t.Name, t.Market, t.Alias, t.SrId,
            (int?)t.Founded,
            t.Owner, t.GeneralManager, t.President, t.Mascot,
            t.Colors,
            t.ChampionshipsWon, t.ConferenceTitles, t.DivisionTitles, t.PlayoffAppearances,
            t.Venue is null ? null : new VenueDto(
                t.Venue.Id, t.Venue.Name, t.Venue.City, t.Venue.State, t.Venue.Country,
                t.Venue.Capacity, t.Venue.Surface, t.Venue.RoofType, t.Venue.Lat, t.Venue.Lng),
            t.Division is null ? null : new DivisionSummaryDto(
                t.Division.Id, t.Division.Name, t.Division.Alias,
                new ConferenceSummaryDto(
                    t.Division.Conference!.Id, t.Division.Conference.Name, t.Division.Conference.Alias)),
            t.Coaches.Select(c => new CoachDto(c.Id, c.FullName, c.Position)).ToList());
    }

    public async Task<IReadOnlyList<PlayerRosterDto>> GetRosterAsync(Guid teamId)
    {
        var players = await db.Players
            .AsNoTracking()
            .Where(p => p.TeamId == teamId)
            .OrderBy(p => p.Position.ToString())
            .ThenBy(p => p.Name)
            .ToListAsync();

        return players
            .Select(p => new PlayerRosterDto(p.Id, p.Name, p.FirstName, p.LastName,
                p.Jersey, p.Position?.ToString(), p.Status, p.Experience))
            .ToList();
    }

    public Task<StatBlockDto?> GetSeasonStatsAsync(Guid teamId, int seasonYear, string seasonType)
        => Task.FromResult<StatBlockDto?>(null);

    private static TeamSummaryDto ToSummary(Team t) => new(
        t.Id, t.Name, t.Market, t.Alias,
        t.Colors,
        t.Division is null ? null : new DivisionSummaryDto(
            t.Division.Id, t.Division.Name, t.Division.Alias,
            new ConferenceSummaryDto(
                t.Division.Conference!.Id, t.Division.Conference.Name, t.Division.Conference.Alias)));
}
