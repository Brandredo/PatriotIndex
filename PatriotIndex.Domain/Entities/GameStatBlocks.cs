using Microsoft.EntityFrameworkCore;

namespace PatriotIndex.Domain.Entities;

// ── Player game stats block ────────────────────────────────────────────────

[Owned]
public class PlayerGameStatsBlock
{
    public SeasonPassingStats?          Passing    { get; set; }
    public SeasonRushingStats?          Rushing    { get; set; }
    public SeasonReceivingStats?        Receiving  { get; set; }
    public SeasonDefenseStats?          Defense    { get; set; }
    public SeasonFieldGoalStats?        FieldGoals { get; set; }
    public SeasonPlayerExtraPointStats? ExtraPoints { get; set; }
    public SeasonPuntStats?             Punts      { get; set; }
    public SeasonKickoffStats?          Kickoffs   { get; set; }
    public SeasonPuntReturnStats?       PuntReturns { get; set; }
    public SeasonKickReturnStats?       KickReturns { get; set; }
    public SeasonIntReturnStats?        IntReturns { get; set; }
    public SeasonFumbleStats?           Fumbles    { get; set; }
    public SeasonPenaltyStats?          Penalties  { get; set; }
}

// ── Team game stats block ──────────────────────────────────────────────────

[Owned]
public class TeamGameStatsBlock
{
    public TeamGameSummary?       Summary    { get; set; }
    public SeasonPassingStats?    Passing    { get; set; }
    public SeasonRushingStats?    Rushing    { get; set; }
    public SeasonReceivingStats?  Receiving  { get; set; }
    public SeasonDefenseStats?    Defense    { get; set; }
    public SeasonFieldGoalStats?  FieldGoals { get; set; }
    public SeasonPuntStats?       Punts      { get; set; }
    public SeasonKickoffStats?    Kickoffs   { get; set; }
    public SeasonPuntReturnStats? PuntReturns { get; set; }
    public SeasonKickReturnStats? KickReturns { get; set; }
    public SeasonIntReturnStats?  IntReturns { get; set; }
    public SeasonFumbleStats?     Fumbles    { get; set; }
    public SeasonPenaltyStats?    Penalties  { get; set; }
}

// ── Per-game team summary (top-level aggregates from statistics.*.summary) ─

[Owned]
public class TeamGameSummary
{
    public string? PossessionTime { get; set; }
    public double  AvgGain        { get; set; }
    public int     Safeties       { get; set; }
    public int     Turnovers      { get; set; }
    public int     PlayCount      { get; set; }
    public int     RushPlays      { get; set; }
    public int     TotalYards     { get; set; }
    public int     Fumbles        { get; set; }
    public int     Penalties      { get; set; }
    public int     ReturnYards    { get; set; }
}
