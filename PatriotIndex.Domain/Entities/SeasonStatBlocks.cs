using Microsoft.EntityFrameworkCore;

namespace PatriotIndex.Domain.Entities;

// ── Container blocks ──────────────────────────────────────────────────────

[Owned]
public class TeamStatBlock
{
    public SeasonTouchdownStats? Touchdowns { get; set; }
    public SeasonRushingStats? Rushing { get; set; }
    public SeasonPassingStats? Passing { get; set; }
    public SeasonReceivingStats? Receiving { get; set; }
    public SeasonDefenseStats? Defense { get; set; }
    public SeasonFieldGoalStats? FieldGoals { get; set; }
    public SeasonKickoffStats? Kickoffs { get; set; }
    public SeasonKickReturnStats? KickReturns { get; set; }
    public SeasonPuntStats? Punts { get; set; }
    public SeasonPuntReturnStats? PuntReturns { get; set; }
    public SeasonInterceptionStats? Interceptions { get; set; }
    public SeasonIntReturnStats? IntReturns { get; set; }
    public SeasonFumbleStats? Fumbles { get; set; }
    public SeasonFirstDownStats? FirstDowns { get; set; }
    public SeasonPenaltyStats? Penalties { get; set; }
    public SeasonMiscReturnStats? MiscReturns { get; set; }
    public SeasonTeamExtraPointStats? ExtraPoints { get; set; }
    public SeasonEfficiencyStats? Efficiency { get; set; }
}

[Owned]
public class PlayerStatBlock
{
    public SeasonRushingStats? Rushing { get; set; }
    public SeasonPassingStats? Passing { get; set; }
    public SeasonReceivingStats? Receiving { get; set; }
    public SeasonDefenseStats? Defense { get; set; }
    public SeasonFieldGoalStats? FieldGoals { get; set; }
    public SeasonPlayerExtraPointStats? ExtraPoints { get; set; }
    public SeasonPuntStats? Punts { get; set; }
    public SeasonKickoffStats? Kickoffs { get; set; }
    public SeasonPuntReturnStats? PuntReturns { get; set; }
    public SeasonKickReturnStats? KickReturns { get; set; }
    public SeasonMiscReturnStats? MiscReturns { get; set; }
    public SeasonFumbleStats? Fumbles { get; set; }
    public SeasonPenaltyStats? Penalties { get; set; }
    public SeasonIntReturnStats? IntReturns { get; set; }
}

// ── Shared stat categories ────────────────────────────────────────────────

[Owned]
public class SeasonTouchdownStats
{
    public int Pass { get; set; }
    public int Rush { get; set; }
    public int TotalReturn { get; set; }
    public int Total { get; set; }
    public int FumbleReturn { get; set; }
    public int IntReturn { get; set; }
    public int KickReturn { get; set; }
    public int PuntReturn { get; set; }
    public int Other { get; set; }
}

[Owned]
public class SeasonRushingStats
{
    public double AvgYards { get; set; }
    public int Attempts { get; set; }
    public int Touchdowns { get; set; }
    public int Tlost { get; set; }
    public int TlostYards { get; set; }
    public int Yards { get; set; }
    public int Longest { get; set; }
    public int LongestTouchdown { get; set; }
    public int RedzoneAttempts { get; set; }
    public int BrokenTackles { get; set; }
    public int KneelDowns { get; set; }
    public int Scrambles { get; set; }
    public int YardsAfterContact { get; set; }
    public int? FirstDowns { get; set; }
}

[Owned]
public class SeasonPassingStats
{
    public int Attempts { get; set; }
    public int Completions { get; set; }
    public double CmpPct { get; set; }
    public int Interceptions { get; set; }
    public int SackYards { get; set; }
    public double Rating { get; set; }
    public int Touchdowns { get; set; }
    public double AvgYards { get; set; }
    public int Sacks { get; set; }
    public int Longest { get; set; }
    public int LongestTouchdown { get; set; }
    public int AirYards { get; set; }
    public int RedzoneAttempts { get; set; }
    public int NetYards { get; set; }
    public int Yards { get; set; }
    public int GrossYards { get; set; }
    public int IntTouchdowns { get; set; }
    public int ThrowAways { get; set; }
    public int PoorThrows { get; set; }
    public int DefendedPasses { get; set; }
    public int DroppedPasses { get; set; }
    public int Spikes { get; set; }
    public int Blitzes { get; set; }
    public int Hurries { get; set; }
    public int Knockdowns { get; set; }
    public double PocketTime { get; set; }
    public int BattedPasses { get; set; }
    public int OnTargetThrows { get; set; }
    public int? FirstDowns { get; set; }
    public double? AvgPocketTime { get; set; }
}

[Owned]
public class SeasonReceivingStats
{
    public int Targets { get; set; }
    public int Receptions { get; set; }
    public double AvgYards { get; set; }
    public int Yards { get; set; }
    public int Touchdowns { get; set; }
    public int YardsAfterCatch { get; set; }
    public int Longest { get; set; }
    public int LongestTouchdown { get; set; }
    public int RedzoneTargets { get; set; }
    public int AirYards { get; set; }
    public int BrokenTackles { get; set; }
    public int DroppedPasses { get; set; }
    public int CatchablePasses { get; set; }
    public int YardsAfterContact { get; set; }
    public int? FirstDowns { get; set; }
}

[Owned]
public class SeasonDefenseStats
{
    public int Tackles { get; set; }
    public int Assists { get; set; }
    public int Combined { get; set; }
    public double Sacks { get; set; }
    public int SackYards { get; set; }
    public int Interceptions { get; set; }
    public int PassesDefended { get; set; }
    public int ForcedFumbles { get; set; }
    public int FumbleRecoveries { get; set; }
    public int QbHits { get; set; }
    public int Tloss { get; set; }
    public int TlossYards { get; set; }
    public int Safeties { get; set; }
    public int SpTackles { get; set; }
    public int SpAssists { get; set; }
    public int SpForcedFumbles { get; set; }
    public int SpFumbleRecoveries { get; set; }
    public int SpBlocks { get; set; }
    public int MiscTackles { get; set; }
    public int MiscAssists { get; set; }
    public int MiscForcedFumbles { get; set; }
    public int MiscFumbleRecoveries { get; set; }
    public int SpOwnFumbleRecoveries { get; set; }
    public int SpOppFumbleRecoveries { get; set; }
    public int ThreeAndOutsForced { get; set; }
    public int FourthDownStops { get; set; }
    public int DefTargets { get; set; }
    public int DefComps { get; set; }
    public int Blitzes { get; set; }
    public int Hurries { get; set; }
    public int Knockdowns { get; set; }
    public int MissedTackles { get; set; }
    public int BattedPasses { get; set; }
}

[Owned]
public class SeasonFieldGoalStats
{
    public int Attempts { get; set; }
    public int Made { get; set; }
    public int Blocked { get; set; }
    public int Yards { get; set; }
    public double AvgYards { get; set; }
    public int Longest { get; set; }
    public int Missed { get; set; }
    public double Pct { get; set; }
    public int Attempts19 { get; set; }
    public int Attempts29 { get; set; }
    public int Attempts39 { get; set; }
    public int Attempts49 { get; set; }
    public int Attempts50 { get; set; }
    public int Made19 { get; set; }
    public int Made29 { get; set; }
    public int Made39 { get; set; }
    public int Made49 { get; set; }
    public int Made50 { get; set; }
}

[Owned]
public class SeasonKickoffStats
{
    public int Kickoffs { get; set; }
    public int Endzone { get; set; }
    public int Inside20 { get; set; }
    public int ReturnYards { get; set; }
    public int Returned { get; set; }
    public int Touchbacks { get; set; }
    public int Yards { get; set; }
    public int OutOfBounds { get; set; }
    public int OnsideAttempts { get; set; }
    public int OnsideSuccesses { get; set; }
    public int SquibKicks { get; set; }
}

[Owned]
public class SeasonKickReturnStats
{
    public double AvgYards { get; set; }
    public int Yards { get; set; }
    public int Longest { get; set; }
    public int Touchdowns { get; set; }
    public int LongestTouchdown { get; set; }
    public int Faircatches { get; set; }
    public int Returns { get; set; }
}

[Owned]
public class SeasonPuntStats
{
    public int Attempts { get; set; }
    public int Yards { get; set; }
    public int NetYards { get; set; }
    public int Blocked { get; set; }
    public int Touchbacks { get; set; }
    public int Inside20 { get; set; }
    public int ReturnYards { get; set; }
    public double AvgNetYards { get; set; }
    public double AvgYards { get; set; }
    public int Longest { get; set; }
    public double HangTime { get; set; }
    public double AvgHangTime { get; set; }
}

[Owned]
public class SeasonPuntReturnStats
{
    public double AvgYards { get; set; }
    public int Returns { get; set; }
    public int Yards { get; set; }
    public int Longest { get; set; }
    public int Touchdowns { get; set; }
    public int LongestTouchdown { get; set; }
    public int Faircatches { get; set; }
}

[Owned]
public class SeasonInterceptionStats
{
    public int ReturnYards { get; set; }
    public int Returned { get; set; }
    public int Interceptions { get; set; }
}

[Owned]
public class SeasonIntReturnStats
{
    public double AvgYards { get; set; }
    public int Yards { get; set; }
    public int Longest { get; set; }
    public int Touchdowns { get; set; }
    public int LongestTouchdown { get; set; }
    public int Returns { get; set; }
}

[Owned]
public class SeasonFumbleStats
{
    public int Fumbles { get; set; }
    public int LostFumbles { get; set; }
    public int OwnRec { get; set; }
    public int OwnRecYards { get; set; }
    public int OppRec { get; set; }
    public int OppRecYards { get; set; }
    public int OutOfBounds { get; set; }
    public int ForcedFumbles { get; set; }
    public int OwnRecTds { get; set; }
    public int OppRecTds { get; set; }
    public int EzRecTds { get; set; }
}

[Owned]
public class SeasonFirstDownStats
{
    public int Pass { get; set; }
    public int Penalty { get; set; }
    public int Rush { get; set; }
    public int Total { get; set; }
}

[Owned]
public class SeasonPenaltyStats
{
    public int Penalties { get; set; }
    public int Yards { get; set; }
    public int? FirstDowns { get; set; }
}

[Owned]
public class SeasonMiscReturnStats
{
    public int Yards { get; set; }
    public int Touchdowns { get; set; }
    public int LongestTouchdown { get; set; }
    public int BlkFgTouchdowns { get; set; }
    public int BlkPuntTouchdowns { get; set; }
    public int FgReturnTouchdowns { get; set; }
    public int EzRecTouchdowns { get; set; }
    public int Returns { get; set; }
}

// ── Extra points — team (nested kicks + conversions) ─────────────────────

[Owned]
public class SeasonTeamExtraPointStats
{
    public SeasonEpKickStats? Kicks { get; set; }
    public SeasonEpConversionStats? Conversions { get; set; }
}

[Owned]
public class SeasonEpKickStats
{
    public int Attempts { get; set; }
    public int Blocked { get; set; }
    public int Made { get; set; }
    public double Pct { get; set; }
}

[Owned]
public class SeasonEpConversionStats
{
    public int PassAttempts { get; set; }
    public int PassSuccesses { get; set; }
    public int RushAttempts { get; set; }
    public int RushSuccesses { get; set; }
    public int DefenseAttempts { get; set; }
    public int DefenseSuccesses { get; set; }
    public int TurnoverSuccesses { get; set; }
}

// ── Extra points — player / kicker (flat) ────────────────────────────────

[Owned]
public class SeasonPlayerExtraPointStats
{
    public int Attempts { get; set; }
    public int Made { get; set; }
    public int Blocked { get; set; }
    public int Missed { get; set; }
    public double Pct { get; set; }
}

// ── Efficiency ────────────────────────────────────────────────────────────

[Owned]
public class SeasonEfficiencyStats
{
    public SeasonEfficiencyBlock? Goaltogo { get; set; }
    public SeasonEfficiencyBlock? Redzone { get; set; }
    public SeasonEfficiencyBlock? Thirddown { get; set; }
    public SeasonEfficiencyBlock? Fourthdown { get; set; }
}

[Owned]
public class SeasonEfficiencyBlock
{
    public int Attempts { get; set; }
    public int Successes { get; set; }
    public double Pct { get; set; }
}
