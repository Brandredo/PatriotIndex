using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Helpers;

namespace PatriotIndex.Domain.Transformers;

public class TeamProfileTransformer(string json)
{
    public Team Transform()
    {
        using var trf = new JsonTraverser(json);

        var team = new Team
        {
            Id = trf.GetGuid("id") ?? throw new Exception("team id is null"),
            Name = trf.GetString("name"),
            Market = trf.GetString("market"),
            Alias = trf.GetString("alias"),
            SrId = trf.GetString("sr_id"),
            Founded = trf.GetShortN("founded"),
            Owner = trf.GetStringN("owner"),
            GeneralManager = trf.GetStringN("general_manager"),
            President = trf.GetStringN("president"),
            Mascot = trf.GetStringN("mascot"),
            VenueId = trf.GetGuid("venue.id") ?? throw new Exception("venue id is null"),
            DivisionId = trf.GetGuid("division.id") ?? throw new Exception("division id is null"),
            ChampionshipsWon = trf.GetIntN("championships_won"),
            ConferenceTitles = trf.GetIntN("conference_titles"),
            DivisionTitles = trf.GetIntN("division_titles"),
            PlayoffAppearances = trf.GetIntN("playoff_appearances"),
            ChampionshipSeasons = trf.GetStringN("championship_seasons"),
        };
        
        return team;
    }
}