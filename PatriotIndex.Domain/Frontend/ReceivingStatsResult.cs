namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// All derived receiving metrics calculated from <c>SeasonReceivingStats</c>.
/// Applies to WRs, TEs, and pass-catching RBs.
/// </summary>
public sealed record ReceivingStatsResult
{
    public int Targets { get; init; }
    public int Receptions { get; init; }
    public decimal AvgYards { get; init; }
    public int Yards { get; init; }
    public int Touchdowns { get; init; }
    public int YardsAfterCatch { get; init; }
    public int Longest { get; init; }
    public int LongestTouchdown { get; init; }
    public int RedzoneTargets { get; init; }
    public int AirYards { get; init; }
    public int BrokenTackles { get; init; }
    public int DroppedPasses { get; init; }
    public int CatchablePasses { get; init; }
    public int YardsAfterContact { get; init; }
    public int? FirstDowns { get; init; }

    // ── Volume &amp; Opportunity ────────────────────────────────────────────────

    /// <summary>Receptions / Targets × 100. Raw catch conversion rate.</summary>
    public double CatchRate { get; init; }

    /// <summary>Receptions / CatchablePasses × 100. Catch rate on catchable balls only; removes QB errors from denominator.</summary>
    public double TrueCatchRate { get; init; }

    /// <summary>RedzoneTargets / Targets × 100. Concentration of targets in scoring zone.</summary>
    public double RedzoneTargetRate { get; init; }

    /// <summary>(Targets − CatchablePasses) / Targets × 100. Share of targets that were uncatchable (QB fault).</summary>
    public double UncatchablePassRate { get; init; }

    // ── Efficiency ────────────────────────────────────────────────────────────

    /// <summary>Yards / Targets. Accounts for incompletions; unbiased per-opportunity productivity.</summary>
    public double YardsPerTarget { get; init; }

    /// <summary>Yards / Receptions. Average gain per catch; reflects route depth + YAC combined.</summary>
    public double YardsPerReception { get; init; }

    /// <summary>AirYards / Targets. Average depth of target (aDOT). High = downfield threat; low = possession/slot role.</summary>
    public double AverageDepthOfTarget { get; init; }

    /// <summary>AirYards / Receptions. Depth of completed catches specifically.</summary>
    public double AirYardsPerReception { get; init; }

    /// <summary>YardsAfterCatch / Receptions. After-catch production; identifies players who create extra value post-catch.</summary>
    public double YacPerReception { get; init; }

    /// <summary>YardsAfterCatch / Yards × 100. Proportion of total yards gained on the ground vs. in the air.</summary>
    public double YacShareOfTotalYards { get; init; }

    /// <summary>YardsAfterContact / Receptions. Physical strength post-catch; differentiates power receivers.</summary>
    public double YardsAfterContactPerReception { get; init; }

    /// <summary>BrokenTackles / Receptions. Elusiveness post-catch; correlates strongly with YAC value.</summary>
    public double BrokenTackleRate { get; init; }

    /// <summary>Touchdowns / Receptions × 100. Scoring rate per catch; flags red-zone specialists.</summary>
    public double TdRate { get; init; }

    /// <summary>FirstDowns / Receptions × 100. Chain-moving efficiency; high rate = reliable possession receiver.</summary>
    public double FirstDownRate { get; init; }

    // ── Ball Security ─────────────────────────────────────────────────────────

    /// <summary>DroppedPasses / CatchablePasses × 100. Drops on catchable balls; cleanest ball-security measure at receiver.</summary>
    public double DropRate { get; init; }
}
