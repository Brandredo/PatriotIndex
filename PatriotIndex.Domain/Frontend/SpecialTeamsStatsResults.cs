namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Derived kickoff metrics from <c>SeasonKickoffStats</c>.
/// </summary>
public sealed record KickoffStatsResult
{
    public int Kickoffs { get; init; }
    public int Endzone { get; init; }
    public int Inside20 { get; init; }
    public int ReturnYards { get; init; }
    public int Returned { get; init; }
    public int Touchbacks { get; init; }
    public int Yards { get; init; }
    public int OutOfBounds { get; init; }
    public int OnsideAttempts { get; init; }
    public int OnsideSuccesses { get; init; }

    /// <summary>Touchbacks / Kickoffs × 100. Primary post-2011 kickoff quality metric. High = strong-legged kicker.</summary>
    public double TouchbackRate { get; init; }

    /// <summary>Yards / Kickoffs. Raw leg-strength measure; context for touchback rate.</summary>
    public double AverageDistance { get; init; }

    /// <summary>Endzone / Kickoffs × 100. Kicks landing in the end zone (prerequisite for a touchback).</summary>
    public double EndzoneRate { get; init; }

    /// <summary>Inside20 / Kickoffs × 100. Pins opponent at poor field position when touchback is not achieved.</summary>
    public double Inside20Rate { get; init; }

    /// <summary>(Touchbacks + Inside20) / Kickoffs × 100. Composite quality: % of kicks that pinned the opponent or gave a touchback start.</summary>
    public double ShortFieldRate { get; init; }

    /// <summary>ReturnYards / Returned. When kicked short, average return distance conceded (blends kick quality + coverage).</summary>
    public double AverageReturnYardsAllowed { get; init; }

    /// <summary>OutOfBounds / Kickoffs × 100. Penalised with a 40-yd spot; indicates poor directional control.</summary>
    public double OutOfBoundsRate { get; init; }

    /// <summary>OnsideSuccesses / OnsideAttempts × 100. Recovery rate on onside kicks.</summary>
    public double OnsideSuccessRate { get; init; }

    /// <summary>SquibKicks raw count. Intentional low-trajectory kicks to neutralise dangerous returners.</summary>
    public int SquibKicks { get; init; }
}

/// <summary>
/// Derived punting metrics from <c>SeasonPuntStats</c>.
/// </summary>
public sealed record PuntingStatsResult
{
    // core stats
    public int Attempts { get; init; }
    public int Yards { get; init; }
    public int NetYards { get; init; }
    public int Blocked { get; init; }
    public int Touchbacks { get; init; }
    public int Inside20 { get; init; }
    public int ReturnYards { get; init; }
    public decimal AvgNetYards { get; init; }
    public decimal AvgYards { get; init; }
    public int Longest { get; init; }
    public double HangTime { get; init; }
    public double AvgHangTime { get; init; }

    /// <summary>Yards / Attempts. Raw kick distance; misleading without context (field position, touchbacks).</summary>
    public double GrossAverage { get; init; }

    /// <summary>NetYards / Attempts. Subtracts return yards; far more meaningful than gross average.</summary>
    public double NetAverage { get; init; }

    /// <summary>AvgYards − AvgNetYards. Spread between distance and net value; high delta = lots of return yards given up.</summary>
    public decimal GrossNetDelta { get; init; }

    /// <summary>NetYards / Yards × 100. Percentage of gross distance converted into net value after returns.</summary>
    public double DistanceEfficiencyIndex { get; init; }

    /// <summary>Inside20 / Attempts × 100. "Coffin corner" rate; elite punters aim for 40%+.</summary>
    public double Inside20Rate { get; init; }

    /// <summary>Touchbacks / Attempts × 100. Over-punted — gifts opponent a 20-yd start. Negative outcome.</summary>
    public double TouchbackRate { get; init; }

    /// <summary>(Inside20 + Touchbacks) / Attempts × 100. Directional quality proxy adapted from analytics community.</summary>
    public double ShortFieldRate { get; init; }

    /// <summary>Blocked / Attempts × 100. Catastrophic play; blocked punts often result in a score.</summary>
    public double BlockedRate { get; init; }

    /// <summary>ReturnYards / Attempts. Return yards conceded per punt — combines hang time, direction, and coverage quality.</summary>
    public double ReturnYardsAllowedPerPunt { get; init; }

    /// <summary>HangTime / Attempts (or stored AvgHangTime). Seconds in air; 4.5s+ gives coverage more time to get downfield.</summary>
    public double AverageHangTime { get; init; }
}

/// <summary>
/// Derived kick-return metrics from <c>SeasonKickReturnStats</c>.
/// </summary>
public sealed record KickReturnStatsResult
{
    // core stats
    public decimal AvgYards { get; init; }
    public int Yards { get; init; }
    public int Longest { get; init; }
    public int Touchdowns { get; init; }
    public int LongestTouchdown { get; init; }
    public int Faircatches { get; init; }
    public int Returns { get; init; }

    /// <summary>Yards / Returns. Primary efficiency stat; 25+ yards is considered excellent.</summary>
    public double AverageReturn { get; init; }

    /// <summary>Touchdowns / Returns × 100. Explosive play rate; even 1 TD per season provides significant hidden value.</summary>
    public double TdRate { get; init; }

    /// <summary>Faircatches / (Returns + Faircatches) × 100. Risk-aversion indicator on kickoffs.</summary>
    public double FairCatchRate { get; init; }
}

/// <summary>
/// Derived punt-return metrics from <c>SeasonPuntReturnStats</c>.
/// </summary>
public sealed record PuntReturnStatsResult
{
    // core stats
    public decimal AvgYards { get; init; }
    public int Returns { get; init; }
    public int Yards { get; init; }
    public int Longest { get; init; }
    public int Touchdowns { get; init; }
    public int LongestTouchdown { get; init; }
    public int Faircatches { get; init; }

    /// <summary>Yards / Returns. Core efficiency stat; 10+ yards is excellent at NFL level.</summary>
    public double AverageReturn { get; init; }

    /// <summary>Touchdowns / Returns × 100. Every punt-return TD is roughly a 7-point swing vs. average field position.</summary>
    public double TdRate { get; init; }

    /// <summary>Faircatches / (Returns + Faircatches) × 100. Punt hang-time pressure indicator; high = coverage limiting return opportunities.</summary>
    public double FairCatchRate { get; init; }
}

/// <summary>
/// Derived interception-return metrics from <c>SeasonIntReturnStats</c>.
/// </summary>
public sealed record IntReturnStatsResult
{
    // core stats
    public decimal AvgYards { get; init; }
    public int Yards { get; init; }
    public int Longest { get; init; }
    public int Touchdowns { get; init; }
    public int LongestTouchdown { get; init; }
    public int Returns { get; init; }

    /// <summary>Yards / Returns. Field awareness and open-field speed after the pick.</summary>
    public double AverageReturn { get; init; }

    /// <summary>Touchdowns / Returns × 100. Pick-6 conversion rate; measures full value provided beyond just the interception.</summary>
    public double PickSixRate { get; init; }
}

/// <summary>
/// Derived miscellaneous-return metrics from <c>SeasonMiscReturnStats</c>.
/// </summary>
public sealed record MiscReturnStatsResult
{
    // core stats
    public int Yards { get; init; }
    public int Touchdowns { get; init; }
    public int LongestTouchdown { get; init; }
    public int Returns { get; init; }

    /// <summary>Yards / Returns. Average yardage on miscellaneous returns (blocked kicks, etc.).</summary>
    public double AverageReturnYards { get; init; }

    /// <summary>BlkFgTouchdowns + BlkPuntTouchdowns + FgReturnTouchdowns + EzRecTouchdowns. All misc ST touchdowns combined.</summary>
    public int TotalMiscTouchdowns { get; init; }

    /// <summary>Blocked field goals returned for a touchdown.</summary>
    public int BlockedFgTouchdowns { get; init; }

    /// <summary>Blocked punts returned for a touchdown — worst single-play outcome for a punting unit.</summary>
    public int BlockedPuntTouchdowns { get; init; }

    /// <summary>Missed field goals returned for a touchdown.</summary>
    public int FgReturnTouchdowns { get; init; }

    /// <summary>End-zone fumble recoveries for a touchdown.</summary>
    public int EzRecTouchdowns { get; init; }
}
