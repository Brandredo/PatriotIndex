namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// All derived rushing metrics calculated from <c>SeasonRushingStats</c>.
/// Applies to QBs (designed runs + scrambles) and RBs/WRs equally.
/// </summary>
public sealed record RushingStatsResult
{
    public double AvgYards { get; init; }
    public int Attempts { get; init; }
    public int Touchdowns { get; init; }
    public decimal TacklesForLoss { get; init; }
    public int TacklesForLossYards { get; init; }
    public int Yards { get; init; }
    public int Longest { get; init; }
    public int LongestTouchdown { get; init; }
    public int RedzoneAttempts { get; init; }
    public int BrokenTackles { get; init; }
    public int KneelDowns { get; init; }
    public int Scrambles { get; init; }
    public int YardsAfterContact { get; init; }
    public int? FirstDowns { get; init; }


    // ── Efficiency ────────────────────────────────────────────────────────────

    /// <summary>Yards / Attempts. Core yards-per-carry efficiency.</summary>
    public double YardsPerCarry { get; init; }

    /// <summary>YardsAfterContact / Attempts. Power and tackle-breaking ability indicator.</summary>
    public double YardsAfterContactPerCarry { get; init; }

    /// <summary>BrokenTackles / Attempts. Elusiveness metric; one of the best indicators of raw contact avoidance.</summary>
    public double BrokenTackleRate { get; init; }

    /// <summary>Touchdowns / Attempts × 100. Scoring rate per carry.</summary>
    public double TdRate { get; init; }

    /// <summary>FirstDowns / Attempts × 100. Chain-moving efficiency per carry.</summary>
    public double FirstDownRate { get; init; }

    // ── Negative Plays ────────────────────────────────────────────────────────

    /// <summary>TacklesForLoss / Attempts × 100. Negative-play rate; high rate signals scheme mismatch or poor blocking.</summary>
    public double TflRate { get; init; }

    /// <summary>TacklesForLossYards / TacklesForLoss. Average depth of backfield penetration per TFL.</summary>
    public double TflYardsPerTfl { get; init; }

    // ── Redzone ───────────────────────────────────────────────────────────────

    /// <summary>RedzoneAttempts / Attempts × 100. Usage concentration in scoring territory.</summary>
    public double RedzoneAttemptRate { get; init; }

    /// <summary>Touchdowns / RedzoneAttempts × 100. Scoring efficiency specifically inside the 20.</summary>
    public double RedzoneTdConversionRate { get; init; }

    // ── QB-Specific ───────────────────────────────────────────────────────────

    /// <summary>Scrambles / Attempts × 100. Proportion of rushes that were unplanned scrambles.</summary>
    public double ScrambleRate { get; init; }

    /// <summary>KneelDowns / Attempts × 100. Clock-kill plays that inflate attempt counts and suppress YPA.</summary>
    public double KneelDownRate { get; init; }

    /// <summary>Yards / (Attempts − KneelDowns). True rushing YPC with clock-kill plays excluded.</summary>
    public double AdjustedYardsPerCarry { get; init; }
}
