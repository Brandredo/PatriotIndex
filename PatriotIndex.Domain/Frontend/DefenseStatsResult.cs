namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// All derived defensive metrics calculated from <c>SeasonDefenseStats</c> and <c>SeasonIntReturnStats</c>.
/// Applies to all defensive positions: DL, LB, DB.
/// </summary>
public sealed record DefenseStatsResult
{
    // core stats
    public int Tackles { get; init; }
    public int Assists { get; init; }
    public int Combined { get; init; }
    public decimal Sacks { get; init; }
    public decimal SackYards { get; init; }
    public int Interceptions { get; init; }
    public int PassesDefended { get; init; }
    public int ForcedFumbles { get; init; }
    public int FumbleRecoveries { get; init; }
    public int QbHits { get; init; }
    public decimal TacklesForLoss { get; init; }
    public decimal TacklesForLossYards { get; init; }
    public int SpTackles { get; init; }
    public int SpAssists { get; init; }
    public int SpForcedFumbles { get; init; }
    public int SpFumbleRecoveries { get; init; }
    public int MiscTackles { get; init; }
    public int MiscAssists { get; init; }
    public int MiscForcedFumbles { get; init; }
    public int MiscFumbleRecoveries { get; init; }
    public int SpOwnFumbleRecoveries { get; init; }
    public int SpOppFumbleRecoveries { get; init; }
    public int DefTargets { get; init; }
    public int DefComps { get; init; }
    public int Blitzes { get; init; }
    public int Hurries { get; init; }
    public int Knockdowns { get; init; }
    public int MissedTackles { get; init; }
    public int BattedPasses { get; init; }

    // ── Tackles &amp; Stops ──────────────────────────────────────────────────────

    /// <summary>Tackles / Combined × 100. Proportion of tackles made unassisted; high = decisive defender.</summary>
    public double SoloTackleRate { get; init; }

    /// <summary>MissedTackles / (Combined + MissedTackles) × 100. Tackling accuracy; predictive of YAC allowed.</summary>
    public double MissedTackleRate { get; init; }

    /// <summary>Tloss / Combined × 100. Percentage of tackles made behind the line of scrimmage.</summary>
    public double TflRate { get; init; }

    /// <summary>TlossYards / Tloss. Average depth of backfield penetration per TFL.</summary>
    public double TflYardsPerTfl { get; init; }

    /// <summary>Sacks / Combined × 100. Proportion of stops resulting in a QB sack.</summary>
    public double SackRate { get; init; }

    /// <summary>SackYards / Sacks. Average yardage loss per sack; deeper sacks are higher impact.</summary>
    public double SackYardsPerSack { get; init; }

    /// <summary>Safeties raw count. Worth 2 points + possession change; rare but high-impact plays.</summary>
    public int Safeties { get; init; }

    /// <summary>FourthDownStops raw count. Clutch defensive stops on 4th down.</summary>
    public int FourthDownStops { get; init; }

    /// <summary>ThreeAndOutsForced raw count. High-leverage defensive stops that deny opponent possessions.</summary>
    public int ThreeAndOutsForced { get; init; }

    // ── Pass Rush (DL / Edge / LB) ───────────────────────────────────────────

    /// <summary>QbHits / Blitzes × 100. QB hit rate when blitzing; complements hurry and sack rates.</summary>
    public double QbHitRateOnBlitz { get; init; }

    /// <summary>Hurries / Blitzes × 100. Disruption rate even without a sack; hurries affect decision-making.</summary>
    public double HurryRateOnBlitz { get; init; }

    /// <summary>Knockdowns / Blitzes × 100. Physical post-release disruption per blitz.</summary>
    public double KnockdownRateOnBlitz { get; init; }

    /// <summary>BattedPasses / Blitzes × 100. Line-of-scrimmage deflection rate; rewards tall DL with quick hands.</summary>
    public double BattedPassRateOnBlitz { get; init; }

    /// <summary>
    /// (Sacks + QbHits + Hurries) / Blitzes × 100.
    /// Composite pass-rush effectiveness: any form of QB pressure per blitz opportunity.
    /// </summary>
    public double TotalPressureRate { get; init; }

    // ── Coverage (DB / LB) ───────────────────────────────────────────────────

    /// <summary>DefComps / DefTargets × 100. Opposing QB completion% when targeting this defender.</summary>
    public double CompletionPctAllowed { get; init; }

    /// <summary>PassesDefended / DefTargets × 100. Active coverage disruption rate (INTs + deflections).</summary>
    public double PassDefenseRate { get; init; }

    /// <summary>Interceptions / DefTargets × 100. INT rate per target; elite corners make QBs avoid them entirely.</summary>
    public double InterceptionRateOnTargets { get; init; }

    /// <summary>
    /// (DefTargets − DefComps − Interceptions) / DefTargets × 100.
    /// True incompletion rate — how often the defender prevented any positive outcome.
    /// </summary>
    public double CompletionAvoidedRate { get; init; }

    // ── Turnovers ─────────────────────────────────────────────────────────────

    /// <summary>ForcedFumbles / Combined × 100. Strip ability per tackle opportunity.</summary>
    public double ForcedFumbleRate { get; init; }

    /// <summary>FumbleRecoveries / (ForcedFumbles + MiscForcedFumbles). Recovery rate after stripping the ball.</summary>
    public double FumbleRecoveryRate { get; init; }

    // ── Interception Returns ─────────────────────────────────────────────────

    /// <summary>IntReturns.Yards / IntReturns.Returns. Average return yards per interception.</summary>
    public double IntReturnAverage { get; init; }

    /// <summary>IntReturns.Touchdowns / IntReturns.Returns × 100. TD conversion rate on INT returns (pick-6 rate).</summary>
    public double PickSixRate { get; init; }

    // ── Special Teams ─────────────────────────────────────────────────────────

    /// <summary>SpTackles / (SpTackles + SpAssists) × 100. Solo tackle rate on special teams coverage.</summary>
    public double SpSoloTackleRate { get; init; }

    /// <summary>SpForcedFumbles / (SpTackles + SpAssists) × 100. Turnover creation on coverage units.</summary>
    public double SpForcedFumbleRate { get; init; }

    /// <summary>SpBlocks raw count. Blocked kicks or punts; high-impact special teams plays.</summary>
    public int SpBlocks { get; init; }
}
