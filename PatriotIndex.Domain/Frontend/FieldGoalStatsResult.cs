namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// All derived field-goal metrics calculated from <c>SeasonFieldGoalStats</c>.
/// </summary>
public sealed record FieldGoalStatsResult
{
    public int Attempts { get; init; }
    public int Made { get; init; }
    public int Blocked { get; init; }
    public int Yards { get; init; }
    public decimal AvgYards { get; init; }
    public int Longest { get; init; }
    public int Missed { get; init; }
    public decimal Pct { get; init; }
    public int Attempts19 { get; init; }
    public int Attempts29 { get; init; }
    public int Attempts39 { get; init; }
    public int Attempts49 { get; init; }
    public int Attempts50 { get; init; }
    public int Made19 { get; init; }
    public int Made29 { get; init; }
    public int Made39 { get; init; }
    public int Made49 { get; init; }
    public int Made50 { get; init; }

    // ── Overall Accuracy ──────────────────────────────────────────────────────

    /// <summary>Made / Attempts × 100. Foundation kicker accuracy stat. League average ~86%.</summary>
    public double OverallFgPct { get; init; }

    /// <summary>Missed / Attempts × 100. Clean miss rate (excludes blocked kicks).</summary>
    public double MissRate { get; init; }

    /// <summary>Blocked / Attempts × 100. Reflects kick speed/trajectory and OL protection gaps.</summary>
    public double BlockRate { get; init; }

    /// <summary>Yards / Attempts. Average attempt distance; essential context for evaluating FG%.</summary>
    public double AverageAttemptDistance { get; init; }

    // ── Distance-Bracket Accuracy ─────────────────────────────────────────────

    /// <summary>Made19 / Attempts19 × 100. Chip-shot range. Any miss here at NFL level is alarming.</summary>
    public double FgPct1To19 { get; init; }

    /// <summary>Made29 / Attempts29 × 100. Short range; expected 95%+ at NFL level.</summary>
    public double FgPct20To29 { get; init; }

    /// <summary>Made39 / Attempts39 × 100. Mid-range; league average ~90%.</summary>
    public double FgPct30To39 { get; init; }

    /// <summary>Made49 / Attempts49 × 100. Most common long-range bracket; significant variance between kicker tiers.</summary>
    public double FgPct40To49 { get; init; }

    /// <summary>Made50 / Attempts50 × 100. Elite range; league average ~62%. Justin Tucker-calibre is 85%+.</summary>
    public double FgPct50Plus { get; init; }

    // ── Advanced / Composite ─────────────────────────────────────────────────

    /// <summary>
    /// Points above league-average conversion probability by distance bracket.
    /// Σ(Made − expected_at_distance) × 3 using the five range buckets.
    /// Requires league-average FG% constants per bracket supplied at calculation time.
    /// The most honest single-number kicker evaluation metric.
    /// </summary>
    public double PointsAboveExpected { get; init; }

    /// <summary>
    /// Weighted accuracy index normalised for attempt difficulty.
    /// Computed as (Σ made_in_bracket × bracket_weight) / (Σ attempts_in_bracket × bracket_weight).
    /// Harder-distance brackets receive higher weights (1–19=1.0, 20–29=1.2, 30–39=1.5, 40–49=1.8, 50+=2.2).
    /// </summary>
    public double DifficultyWeightedFgPct { get; init; }
}
