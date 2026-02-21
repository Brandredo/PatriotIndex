using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Mappings;

public static class TeamMappings
{
    public static ConferenceSummaryDto ToConferenceSummary(this Conference c) =>
        new(c.Id, c.Name, c.Alias);

    public static DivisionSummaryDto ToDivisionSummary(this Division d) =>
        new(d.Id, d.Name, d.Alias, d.Conference!.ToConferenceSummary());

    public static VenueDto ToVenueDto(this Venue v) =>
        new(v.Id, v.Name, v.City, v.State, v.Country, v.Capacity, v.Surface, v.RoofType, v.Lat, v.Lng);

    public static TeamSummaryDto ToSummary(this Team t) =>
        new(t.Id, t.Name, t.Market, t.Alias, t.PrimaryColor, t.SecondaryColor,
            t.Division != null ? t.Division.ToDivisionSummary() : null);

    public static TeamDetailDto ToDetail(this Team t) =>
        new(t.Id, t.Name, t.Market, t.Alias, t.SrId,
            t.Founded, t.Owner, t.GeneralManager, t.President, t.Mascot,
            t.PrimaryColor, t.SecondaryColor,
            t.ChampionshipsWon, t.ConferenceTitles, t.DivisionTitles, t.PlayoffAppearances,
            t.Venue?.ToVenueDto(),
            t.Division?.ToDivisionSummary(),
            t.Coaches.Select(c => new CoachDto(c.Id, c.FullName, c.Position)).ToList());

    public static PlayerRosterDto ToRosterDto(this Player p) =>
        new(p.Id, p.Name, p.FirstName, p.LastName, p.Jersey, p.Position, p.Status, p.Experience);
}
