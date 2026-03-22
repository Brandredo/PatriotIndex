namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// All derived passing metrics calculated from <c>SeasonPassingStats</c>.
/// Null indicates that the underlying stat block was unavailable.
/// </summary>
public sealed record PassingStatsResult
{

    // Core Stats
    public required int Attempts { get; init; }
    public int Completions { get; init; }
    public int Interceptions { get; init; }
    public int SackYards { get; init; }
    public decimal Rating { get; init; }
    public int Touchdowns { get; init; }
    public decimal AvgYards { get; init; }
    public int Sacks { get; init; }
    public int Longest { get; init; }
    public int LongestTouchdown { get; init; }
    public int AirYards { get; init; }
    public int RedzoneAttempts { get; init; }
    public int NetYards { get; init; }
    public int Yards { get; init; }
    public int GrossYards { get; init; }
    public int IntTouchdowns { get; init; }
    public int ThrowAways { get; init; }
    public int PoorThrows { get; init; }
    public int DefendedPasses { get; init; }
    public int DroppedPasses { get; init; }
    public int Spikes { get; init; }
    public int Blitzes { get; init; }
    public int Hurries { get; init; }
    public int Knockdowns { get; init; }
    public double PocketTime { get; init; }
    public int BattedPasses { get; init; }
    public int OnTargetThrows { get; init; }
    public int? FirstDowns { get; init; }
    public double? AvgPocketTime { get; init; }


    // ── Basic Ratios ──────────────────────────────────────────────────────────

    /// <summary>Completions / Attempts × 100. Foundational accuracy measure.</summary>
    public double CompletionPct { get; init; }

    /// <summary>Yards / Attempts. Average distance gained per pass attempt.</summary>
    public double YardsPerAttempt { get; init; }

    /// <summary>Touchdowns / Attempts × 100. Scoring efficiency per dropback.</summary>
    public double TdPct { get; init; }

    /// <summary>Interceptions / Attempts × 100. Ball-protection measure per attempt.</summary>
    public double IntPct { get; init; }

    /// <summary>Touchdowns / Interceptions. Elite QBs routinely exceed 3.0.</summary>
    public double TdIntRatio { get; init; }

    /// <summary>Sacks / (Attempts + Sacks) × 100. True sack rate including sacks in denominator.</summary>
    public double SackRate { get; init; }

    /// <summary>SackYards / Sacks. Average yardage lost per sack event.</summary>
    public double SackYardsPerSack { get; init; }

    /// <summary>GrossYards − NetYards. Total yardage lost to sacks over the period.</summary>
    public int GrossNetYardsDelta { get; init; }

    /// <summary>ThrowAways / Attempts × 100. Intentional incompletions under pressure.</summary>
    public double ThrowAwayRate { get; init; }

    /// <summary>Spikes / Attempts × 100. Clock-management signal; correlates with 2-minute drill usage.</summary>
    public double SpikeRate { get; init; }

    /// <summary>BattedPasses / Attempts × 100. Low release point or DL height-advantage indicator.</summary>
    public double BattedPassRate { get; init; }

    /// <summary>DefendedPasses / Attempts × 100. How often throws were actively contested.</summary>
    public double DefendedPassRate { get; init; }

    /// <summary>Knockdowns / Attempts × 100. Physical post-release disruption proxy for OL quality.</summary>
    public double KnockdownRate { get; init; }

    /// <summary>Blitzes / Attempts × 100. How often the defense sent extra rushers.</summary>
    public double BlitzRate { get; init; }

    /// <summary>FirstDowns / Completions × 100. Chain-moving efficiency of completions.</summary>
    public double FirstDownPassRate { get; init; }

    /// <summary>Average time from snap to release in seconds.</summary>
    public double AveragePocketTime { get; init; }

    // ── Advanced / Composite ─────────────────────────────────────────────────

    /// <summary>
    /// Official NFL passer rating (scale 0–158.3).
    /// Formula: ((a+b+c+d) / 6) × 100 where each component is clamped to [0, 2.375].
    /// a = (Cmp/Att − 0.3) × 5  |  b = (Yds/Att − 3) × 0.25
    /// c = (TD/Att) × 20         |  d = 2.375 − (INT/Att × 25)
    /// </summary>
    public double PasserRating { get; init; }

    /// <summary>(Yards − SackYards) / (Attempts + Sacks). More honest than raw YPA; penalises sacks.</summary>
    public double NetYardsPerAttempt { get; init; }

    /// <summary>
    /// Adjusted Net Yards per Attempt (ANY/A) — Pro-Football-Reference signature metric.
    /// (Yards + 20×TD − 45×INT − SackYards) / (Attempts + Sacks).
    /// Strong single-number predictor of team winning percentage.
    /// </summary>
    public double AdjustedNetYardsPerAttempt { get; init; }

    /// <summary>OnTargetThrows / (Attempts − ThrowAways − Spikes) × 100. Filters out intentional incompletions.</summary>
    public double OnTargetThrowRate { get; init; }

    /// <summary>PoorThrows / Attempts × 100. Mechanically bad throws regardless of completion.</summary>
    public double PoorThrowRate { get; init; }

    /// <summary>DroppedPasses / Attempts × 100. Receiver drops that suppress recorded completion percentage.</summary>
    public double DropRate { get; init; }

    /// <summary>(Completions + DroppedPasses) / Attempts × 100. Completion% if receivers caught every accurate throw.</summary>
    public double DropAdjustedCompletionPct { get; init; }

    /// <summary>Hurries / Blitzes × 100. How effective blitz packages are at generating QB disruption.</summary>
    public double HurryRateWhenBlitzed { get; init; }

    // ── Air Yards &amp; Depth ──────────────────────────────────────────────────────

    /// <summary>AirYards / Attempts. Average depth of target (aDOT). High = deep passer; low = check-down tendency.</summary>
    public double AverageDepthOfTarget { get; init; }

    /// <summary>AirYards / Completions. Depth of the average completed pass.</summary>
    public double AirYardsPerCompletion { get; init; }

    /// <summary>(Yards − AirYards) / Completions. Approximate yards-after-catch contributed per completion.</summary>
    public double YacPerCompletion { get; init; }

    /// <summary>RedzoneAttempts / Attempts × 100. Scoring-zone usage concentration.</summary>
    public double RedzoneAttemptRate { get; init; }
}
