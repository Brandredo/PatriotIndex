using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Mappings;

public static class PbpMappings
{
    public static PlayStatDto ToDto(this PlayStat s, string? playerName, string? teamAlias) =>
        new(s.Id, s.StatType, s.PlayerId, playerName, s.TeamId, teamAlias,
            s.Yards, s.Attempt, s.Complete, s.Touchdown,
            s.Interception, s.Fumble, s.Sack, s.Touchback, s.Category);

    public static PlayDto ToDto(this Play p, Dictionary<Guid, string> teamAliases,
        Dictionary<Guid, string> playerNames) =>
        new(p.Id, p.Sequence, p.Clock, p.PlayType, p.Description,
            p.HomePoints, p.AwayPoints, p.Down, p.Yfd,
            p.PossessionTeamId,
            p.PossessionTeamId.HasValue ? teamAliases.GetValueOrDefault(p.PossessionTeamId.Value) : null,
            p.StartYardline,
            p.StartSideId.HasValue ? teamAliases.GetValueOrDefault(p.StartSideId.Value) : null,
            p.EndYardline,
            p.EndSideId.HasValue ? teamAliases.GetValueOrDefault(p.EndSideId.Value) : null,
            p.Scoring, p.Turnover, p.FirstDown, p.Penalty,
            p.FakePunt, p.FakeFg, p.ScreenPass, p.PlayAction, p.Rpo,
            p.HashMark, p.WallClock,
            p.PlayStats.Select(s => s.ToDto(
                s.PlayerId.HasValue ? playerNames.GetValueOrDefault(s.PlayerId.Value) : null,
                s.TeamId.HasValue ? teamAliases.GetValueOrDefault(s.TeamId.Value) : null
            )).ToList());

    public static DriveDto ToDto(this Drive d, Dictionary<Guid, string> teamAliases,
        Dictionary<Guid, string> playerNames) =>
        new(d.Id, d.PeriodNumber, d.Sequence,
            d.StartReason, d.EndReason, d.PlayCount, d.Duration, d.FirstDowns, d.NetYards,
            d.StartClock, d.EndClock,
            d.OffensiveTeamId,
            d.OffensiveTeamId.HasValue ? teamAliases.GetValueOrDefault(d.OffensiveTeamId.Value) : null,
            d.DefensiveTeamId,
            d.DefensiveTeamId.HasValue ? teamAliases.GetValueOrDefault(d.DefensiveTeamId.Value) : null,
            d.OffensivePoints, d.DefensivePoints,
            d.Plays.OrderBy(p => p.Sequence)
                   .Select(p => p.ToDto(teamAliases, playerNames)).ToList());
}
