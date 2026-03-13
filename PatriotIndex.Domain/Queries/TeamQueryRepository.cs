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
    
    public async Task<IReadOnlyList<TeamSummaryWithRosterDto>> GetTeamsAndPlayers()
    {
        var teams = await db.Teams
            .AsNoTracking()
            .Include(t => t.Players)
            .Where(t => t.IsActive)
            .OrderBy(t => t.Market)
            .ToListAsync();

        return teams.Select(ToSummaryWithRoster).ToList();
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

    public async Task<StatBlockDto?> GetSeasonStatsAsync(Guid teamId, int seasonYear, string seasonType)
    {
        var stats = await db.TeamSeasonStats
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.TeamId == teamId
                                      && s.SeasonYear == seasonYear
                                      && s.SeasonType == seasonType);

        if (stats is null) return null;

        var r = stats.Record;
        return new StatBlockDto(
            r.Passing?.Attempts ?? 0,
            r.Passing?.Completions ?? 0,
            r.Passing?.Yards ?? 0,
            r.Passing?.Touchdowns ?? 0,
            r.Passing?.Interceptions ?? 0,
            (double)(r.Passing?.Rating ?? 0),
            r.Passing?.Sacks ?? 0,
            r.Passing?.SackYards ?? 0,
            r.Rushing?.Attempts ?? 0,
            r.Rushing?.Yards ?? 0,
            r.Rushing?.Touchdowns ?? 0,
            r.Rushing?.AvgYards ?? 0,
            r.Rushing?.Longest ?? 0,
            r.Receiving?.Targets ?? 0,
            r.Receiving?.Receptions ?? 0,
            r.Receiving?.Yards ?? 0,
            r.Receiving?.Touchdowns ?? 0,
            (double)(r.Receiving?.AvgYards ?? 0),
            r.Receiving?.Longest ?? 0,
            r.Defense?.Tackles ?? 0,
            r.Defense?.Assists ?? 0,
            (double)(r.Defense?.Sacks ?? 0),
            r.Defense?.Interceptions ?? 0,
            r.Defense?.ForcedFumbles ?? 0,
            r.Defense?.PassesDefended ?? 0,
            r.Defense?.QbHits ?? 0,
            r.FieldGoals?.Attempts ?? 0,
            r.FieldGoals?.Made ?? 0,
            r.FieldGoals?.Longest ?? 0,
            r.ExtraPoints?.Kicks?.Attempts ?? 0,
            r.ExtraPoints?.Kicks?.Made ?? 0,
            r.Punts?.Attempts ?? 0,
            r.Punts?.Yards ?? 0,
            (double)(r.Punts?.AvgYards ?? 0));
    }

    public async Task<IReadOnlyList<TeamPlayerStatsDto>> GetTeamPlayerStatsAsync(
        Guid teamId, int seasonYear, string seasonType)
    {
        var stats = await db.PlayerSeasonStats
            .AsNoTracking()
            .Include(s => s.Player)
            .Where(s => s.TeamId == teamId
                        && s.SeasonYear == seasonYear
                        && s.SeasonType == seasonType)
            .OrderBy(s => s.Player!.Position.ToString())
            .ThenBy(s => s.Player!.Name)
            .ToListAsync();

        return stats.Select(s => new TeamPlayerStatsDto(
            s.PlayerId,
            s.Player?.Name,
            s.Player?.Jersey,
            s.Player?.Position?.ToString(),
            s.GamesPlayed,
            s.GamesStarted,
            PlayerQueryRepository.MapStatBlock(s.Stats)
        )).ToList();
    }

    private static TeamSummaryDto ToSummary(Team t) => new(
        t.Id, t.Name, t.Market, t.Alias,
        t.Colors,
        t.Division is null ? null : new DivisionSummaryDto(
            t.Division.Id, t.Division.Name, t.Division.Alias,
            new ConferenceSummaryDto(
                t.Division.Conference!.Id, t.Division.Conference.Name, t.Division.Conference.Alias)));
    
    private static TeamSummaryWithRosterDto ToSummaryWithRoster(Team t) => new(
        t.Id, t.Name, t.Market, t.Alias,
        t.Players.Select(p => new PlayerMinDto(p.Id, p.Name)).ToList());
}
