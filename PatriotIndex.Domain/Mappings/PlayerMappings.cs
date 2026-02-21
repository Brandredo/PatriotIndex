using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Mappings;

public static class PlayerMappings
{
    public static StatBlockDto ToStatBlock(this PlayerSeasonStats s) =>
        new(s.PassAtt, s.PassCmp, s.PassYds, s.PassTd, s.PassInt, s.PassRating, s.PassSacks, s.PassSackYds,
            s.RushAtt, s.RushYds, s.RushTd, s.RushAvg, s.RushLong,
            s.RecTargets, s.RecReceptions, s.RecYds, s.RecTd, s.RecAvg, s.RecLong,
            s.DefTackles, s.DefAssists, s.DefSacks, s.DefInterceptions,
            s.DefForcedFumbles, s.DefPassesDefended, s.DefQbHits,
            s.FgAtt, s.FgMade, s.FgLong, s.XpAtt, s.XpMade,
            s.PuntAtt, s.PuntYds, s.PuntAvg);

    public static StatBlockDto ToStatBlock(this PlayerGameStats s) =>
        new(s.PassAtt, s.PassCmp, s.PassYds, s.PassTd, s.PassInt, s.PassRating, s.PassSacks, s.PassSackYds,
            s.RushAtt, s.RushYds, s.RushTd, s.RushAvg, s.RushLong,
            s.RecTargets, s.RecReceptions, s.RecYds, s.RecTd, s.RecAvg, s.RecLong,
            s.DefTackles, s.DefAssists, s.DefSacks, s.DefInterceptions,
            s.DefForcedFumbles, s.DefPassesDefended, s.DefQbHits,
            s.FgAtt, s.FgMade, s.FgLong, s.XpAtt, s.XpMade,
            s.PuntAtt, s.PuntYds, s.PuntAvg);

    public static PlayerSeasonStatsDto ToDto(this PlayerSeasonStats s) =>
        new(s.Id, s.SeasonYear, s.SeasonType, s.GamesPlayed, s.GamesStarted, s.ToStatBlock());

    public static PlayerSummaryDto ToSummary(this Player p) =>
        new(p.Id, p.Name, p.FirstName, p.LastName, p.Jersey, p.Position, p.Status,
            p.Team?.ToSummary());

    public static PlayerDetailDto ToDetail(this Player p) =>
        new(p.Id, p.Name, p.FirstName, p.LastName, p.Jersey, p.Position, p.Status, p.Experience,
            p.Height, p.Weight,
            p.BirthDate?.ToString("yyyy-MM-dd"),
            p.College, p.RookieYear, p.Salary, p.SrId,
            p.DraftYear, p.DraftRound, p.DraftPick,
            p.DraftTeam != null ? $"{p.DraftTeam.Market} {p.DraftTeam.Name}" : null,
            p.Team?.ToSummary(),
            p.SeasonStats.Select(s => s.ToDto()).ToList());
}
