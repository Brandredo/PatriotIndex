namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Derived fumble metrics from <c>SeasonFumbleStats</c>.
/// </summary>
public sealed record FumbleStatsResult
{
    // core stats
    public int Fumbles { get; init; }
    public int LostFumbles { get; init; }
    public int OwnRec { get; init; }
    public int OwnRecYards { get; init; }
    public int OppRec { get; init; }
    public int OppRecYards { get; init; }
    public int OutOfBounds { get; init; }
    public int OwnRecTds { get; init; }
    public int OppRecTds { get; init; }
    public int EzRecTds { get; init; }

    /// <summary>LostFumbles / Fumbles × 100. Once fumbled, how often does the opponent recover.</summary>
    public double LostFumbleRate { get; init; }

    /// <summary>Fumbles / total touches (rushing + receiving). Requires touch counts supplied externally.</summary>
    /// <remarks>Set by the parent calculator when touch totals are available.</remarks>
    public double? FumbleRatePerTouch { get; init; }

    /// <summary>OwnRecTds + OppRecTds + EzRecTds. Total touchdowns scored from fumble recoveries.</summary>
    public int TotalFumbleRecoveryTds { get; init; }

    /// <summary>ForcedFumbles raw count. Strip-sack / forced fumble total.</summary>
    public int ForcedFumbles { get; init; }
}

/// <summary>
/// Derived penalty metrics from <c>SeasonPenaltyStats</c>.
/// </summary>
public sealed record PenaltyStatsResult
{
    public int Penalties { get; init; }
    public int Yards { get; init; }
    public int? FirstDowns { get; init; }

    /// <summary>Yards / Penalties. Average cost per infraction.</summary>
    public double YardsPerPenalty { get; init; }

    /// <summary>FirstDowns / Penalties × 100. What fraction of opponent penalties moved the chains. Team-level only.</summary>
    public double? FirstDownRate { get; init; }
}

/// <summary>
/// Derived extra-point metrics for a player (kicker) from <c>SeasonPlayerExtraPointStats</c>.
/// </summary>
public sealed record PlayerExtraPointStatsResult
{
    public int Attempts { get; init; }
    public int Made { get; init; }
    public int Blocked { get; init; }
    public int Missed { get; init; }
    public decimal Pct { get; init; }

    /// <summary>Made / Attempts × 100. PAT reliability. League average ~94% since 2015 rule change.</summary>
    public double XpPct { get; init; }

    /// <summary>Blocked / Attempts × 100. Rare but high-impact; a blocked PAT can swing momentum.</summary>
    public double BlockRate { get; init; }

    /// <summary>Missed / Attempts × 100. Clean misses (not blocked).</summary>
    public double MissRate { get; init; }
}

/// <summary>
/// Derived extra-point metrics at team level from <c>SeasonTeamExtraPointStats</c>.
/// Covers both PAT kicks and two-point conversion attempts.
/// </summary>
public sealed record TeamExtraPointStatsResult
{
    // ── PAT Kick ──────────────────────────────────────────────────────────────

    /// <summary>Kicks.Made / Kicks.Attempts × 100.</summary>
    public double XpKickPct { get; init; }

    /// <summary>Kicks.Blocked / Kicks.Attempts × 100.</summary>
    public double XpKickBlockRate { get; init; }

    // ── Two-Point Conversions ─────────────────────────────────────────────────

    /// <summary>PassSuccesses / PassAttempts × 100. Two-point pass play conversion rate.</summary>
    public double TwoPointPassConvPct { get; init; }

    /// <summary>RushSuccesses / RushAttempts × 100. Two-point rush play conversion rate.</summary>
    public double TwoPointRushConvPct { get; init; }

    /// <summary>DefenseSuccesses / DefenseAttempts × 100. Defensive two-point conversions (INT/fumble returned for 2).</summary>
    public double TwoPointDefenseConvPct { get; init; }

    /// <summary>(PassSuccesses + RushSuccesses) / (PassAttempts + RushAttempts) × 100. Combined offensive two-point efficiency. League average ~48–52%.</summary>
    public double OverallTwoPointConvPct { get; init; }

    /// <summary>TurnoverSuccesses raw count. Two-point conversions via turnovers on 2PT attempts.</summary>
    public int TwoPointTurnoverSuccesses { get; init; }
}
