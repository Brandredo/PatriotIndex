using PatriotIndex.Domain.Frontend;

namespace PatriotIndex.Domain.DTOs;

public record PlayerStatsSummaryDto(
    Guid PlayerId,
    string? PlayerName,
    string? Position,
    Guid? TeamId,
    string? TeamAlias,
    string? TeamMarket,
    int GamesPlayed,
    int GamesStarted,
    PlayerStatsResult PlayerStatsResult
    // // Passing — core
    // int? PassAtt,
    // int? PassCmp,
    // decimal? PassCmpPct,
    // int? PassYds,
    // decimal? PassYdsPerGame,
    // int? PassTd,
    // int? PassInt,
    // decimal? PassRating,
    // int? PassSacks,
    // // Passing — advanced
    // int? AirYds,
    // decimal? AirYdsPerAtt,
    // double? AvgPocketTime,
    // decimal? PoorThrowPct,
    // int? Blitzes,
    // int? Hurries,
    // // Rushing — core
    // int? RushAtt,
    // int? RushYds,
    // decimal? RushYpc,
    // int? RushTd,
    // // Rushing — advanced
    // int? BrokenTackles,
    // decimal? BrokenTackleRate,
    // int? YardsAfterContact,
    // decimal? YacPerRush,
    // // Receiving — core
    // int? RecTargets,
    // int? RecReceptions,
    // int? RecYds,
    // decimal? RecAvg,
    // int? RecTd,
    // decimal? CatchPct,
    // // Receiving — advanced
    // int? Yac,
    // decimal? YacPerRec,
    // int? RecAirYds,
    // int? DroppedPasses,
    // decimal? DropRate,
    // // Defense — core
    // int? DefTackles,
    // int? DefAssists,
    // decimal? DefSacks,
    // int? DefInts,
    // int? DefPd,
    // int? DefForcedFumbles,
    // int? DefQbHits,
    // // Defense — advanced
    // int? DefMissedTackles,
    // int? DefBlitzes,
    // int? DefHurries,
    // int? DefKnockdowns,
    // // Kicker
    // int? FgMade,
    // int? FgAtt,
    // decimal? FgPct,
    // int? FgLong,
    // int? FgMade19,
    // int? FgAtt19,
    // int? FgMade29,
    // int? FgAtt29,
    // int? FgMade39,
    // int? FgAtt39,
    // int? FgMade49,
    // int? FgAtt49,
    // int? FgMade50,
    // int? FgAtt50,
    // int? XpMade,
    // int? XpAtt,
    // // Punter
    // int? PuntAtt,
    // decimal? PuntAvg,
    // decimal? PuntNetAvg,
    // int? PuntsInside20,
    // double? AvgHangTime
);
