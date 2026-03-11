using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Helpers;

namespace PatriotIndex.Domain.Transformers;

public class TeamProfileTransformer(string json)
{
    private Team _team;

    public Team Transform()
    {
        using var trf = new JsonTraverser(json);

        _team = new Team
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
        _team.Players = TransformPlayers(trf);
        _team.Coaches = TransformCoaches(trf);
        _team.Venue = TransformVenue(trf);
        _team.Colors = TransformColor(trf);

        return _team;
    }

    private ICollection<Player> TransformPlayers(JsonTraverser trf)
    {
        var players = trf.GetArrayList("players", p => new Player
        {
            Id = p.GetGuid("id") ?? throw new Exception("player id is null"),
            TeamId = _team.Id,
            FirstName = p.GetString("first_name"),
            LastName = p.GetString("last_name"),
            Name = p.GetStringN("name"),
            Jersey = p.GetStringN("jersey"),
            Position = p.GetEnum<PlayerPosition>("position"),
            Height = p.GetIntN("height"),
            Weight = p.GetDecimalN("weight"),
            BirthDate = p.GetDateOnly("birth_date"), // might throw an error
            College = p.GetStringN("college"),
            RookieYear = p.GetIntN("rookie_year"),
            Status = p.GetStringN("status"),
            Experience = p.GetIntN("experience"),
            Salary = p.GetLongN("salary"),
            SrId = p.GetStringN("sr_id"),
            DraftYear = p.GetIntN("draft.year"),
            DraftRound = p.GetIntN("draft.round"),
            DraftPick = p.GetIntN("draft.number"),
            DraftTeamId = p.GetGuid("draft.team.id")
            // Team = null,
            // DraftTeam = null,
            // SeasonStats = null,
            // GameStats = null
        });

        return players;
    }


    private ICollection<Coach> TransformCoaches(JsonTraverser trf)
    {
        var coaches = trf.GetArrayList("coaches", c => new Coach
        {
            Id = c.GetGuid("id") ?? throw new Exception("coach id is null"),
            TeamId = _team.Id,
            FullName = c.GetStringN("full_name"),
            FirstName = c.GetStringN("first_name"),
            LastName = c.GetStringN("last_name"),
            Position = c.GetStringN("position")
            //Team = null
        });

        return coaches;
    }

    private Venue TransformVenue(JsonTraverser trf)
    {
        var v = trf.Scope("venue");
        if (v is null) throw new Exception("venue is null");
        return new Venue
        {
            Id = v.GetGuid("id") ?? throw new Exception("venue id is null"),
            Name = v.GetString("name"),
            City = v.GetStringN("city"),
            State = v.GetStringN("state"),
            Country = v.GetStringN("country"),
            Capacity = v.GetIntN("capacity"),
            Surface = v.GetStringN("surface"),
            Zip = v.GetStringN("zip"),
            Lat = v.GetStringN("location.lat"),
            Lng = v.GetStringN("location.lng"),
            Address = v.GetStringN("address"),
            RoofType = v.GetStringN("roof_type"),
            SrId = v.GetStringN("sr_id")
        };
    }

    private TeamColors? TransformColor(JsonTraverser trf)
    {
        var tc = trf.GetArrayList("team_colors", c => new
        {
            Type = c.GetStringN("type"),
            HexColor = c.GetStringN("hex_color")
        });

        if(tc is null || tc.Count == 0) return null;

        var colors = new TeamColors();

        if (tc.Any(t => t.Type == "primary"))
            colors.Primary = tc.First(t => t.Type == "primary").HexColor;
        if (tc.Any(t => t.Type == "secondary"))
            colors.Secondary = tc.First(t => t.Type == "secondary").HexColor;

        return colors;
    }
}