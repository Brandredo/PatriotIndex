using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion.Converters.Transformers;

public class TeamTransformer(PlayerTransformer playerTransformer, CoachTransformer coachTransformer) : IJsonTransformer<Team>
{
    public Team Transform(JsonNavigator nav)
    {

        var team = new Team
        {
            Id = nav["id"].GetGuid(),
            Name = nav["name"].GetString(),
            Market = nav["market"].GetString(),
            Alias = nav["alias"].GetString(),
            SrId = nav["sr_id"].GetString(),
            Founded = nav.Optional("founded")?.GetInt16() ?? null,
            Owner = nav.Optional("owner")?.GetString() ?? null,
            GeneralManager = nav.Optional("general_manager")?.GetString() ?? null,
            President = nav.Optional("president")?.GetString(),
            Mascot = nav.Optional("mascot")?.GetString() ?? null,
            // mascot
            // fight song
            ChampionshipsWon = nav.Optional("championships_won")?.GetInt32() ?? null,
            ChampionshipSeasons = nav.Optional("championship_seasons")?.GetString() ?? null,
            ConferenceTitles = nav.Optional("conference_titles")?.GetInt32() ?? null,
            DivisionTitles = nav.Optional("division_titles")?.GetInt32() ?? null,
            PlayoffAppearances = nav.Optional("playoff_appearances")?.GetInt32() ?? null,
            VenueId = nav["venue"]["id"].GetGuid(),
            DivisionId = nav["division"]["id"].GetGuid(),
            // SecondaryColor = nav["secondary_color"].GetString(),
        };
        
        
        // transform the players of the team
        foreach (var playerNav in nav["players"].EnumerateArray())
        {
            var p = playerTransformer.Transform(playerNav);
            p.TeamId = team.Id;
            team.Players.Add(p);
        }
        
        // transform the coaches of the team
        foreach (var coachNav in nav["coaches"].EnumerateArray())
        {
            var c = coachTransformer.Transform(coachNav);
            c.TeamId = team.Id;
            team.Coaches.Add(c);
        }


        var colors = new TeamColors();
        // transform the team's colors
        foreach (var colorNav in nav["team_colors"].EnumerateArray())
        {
            if (colorNav["type"].GetString() == "primary")
            {
                colors.Primary = colorNav["hex_color"].GetString();
            }
            else
            {
                colors.Secondary = colorNav["hex_color"].GetString();
            }
        }
        
        team.Colors = colors;

        team.Venue = new Venue
        {
            Id = nav["venue"]["id"].GetGuid(),
            Name = nav["venue"]["name"].GetString(),
            City = nav["venue"].Optional("city")?.GetString() ?? null,
            State = nav["venue"].Optional("state")?.GetString() ?? null,
            Country = nav["venue"].Optional("country")?.GetString() ?? null,
            Capacity = nav["venue"].Optional("capacity")?.GetInt32() ?? null,
            Surface = nav["venue"].Optional("surface")?.GetString() ?? null,
            RoofType = nav["venue"].Optional("roof_type")?.GetString() ?? null,
            SrId = nav["venue"].Optional("sr_id")?.GetString() ?? null,
            Lat = nav["venue"]["location"].Optional("lat")?.GetString() ?? null,
            Lng = nav["venue"]["location"].Optional("lng")?.GetString() ?? null,
            Address = nav["venue"].Optional("address")?.GetString() ?? null,
            Zip = nav["venue"].Optional("zip")?.GetString() ?? null,
        };
        
        
        return team;

    }
}