using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Frontend;

namespace PatriotIndex.Domain.DTOs;

public record TeamStatsSummaryDto(
    Guid TeamId,
    string? TeamName,
    string? TeamMarket,
    string? TeamAlias,
    TeamColors? Colors,
    int GamesPlayed,
    TeamStatsResult TeamStatsResult
    // Core — scoring
    // decimal PtsPerGame,
    // decimal OppPtsPerGame,
    // // Core — yards
    // decimal TotalYardsPerGame,
    // decimal PassYardsPerGame,
    // decimal RushYardsPerGame,
    // // Core — efficiency
    // int TurnoverDiff,
    // decimal ThirdDownPct,
    // decimal RedZonePct,
    // decimal GoalToGoPct,
    // // Core — misc
    // decimal PenaltiesPerGame,
    // decimal PenaltyYardsPerGame,
    // decimal TimeOfPossessionAvg,  // seconds; aggregated from game logs
    // // Advanced
    // decimal PointsPerDrive,
    // decimal YardsPerDrive,
    // decimal PlaysPerDrive,
    // int ThreeAndOutsForced,
    // int MissedTackles,
    // int DefBlitzes,
    // decimal PressureRate   // (hurries + sacks + knockdowns) / opp_pass_att * 100
);
