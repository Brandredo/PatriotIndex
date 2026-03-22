using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Calculates all derived rushing metrics from a <see cref="SeasonRushingStats"/> block.
/// </summary>
public static class RushingCalculator
{
    /// <summary>
    /// Calculates all derived rushing metrics. Returns <c>null</c> if <paramref name="stats"/> is <c>null</c>.
    /// </summary>
    public static RushingStatsResult? Calculate(SeasonRushingStats? stats)
    {
        if (stats is null) return null;

        int att  = stats.Attempts;
        int yds  = stats.Yards;
        int kd   = stats.KneelDowns;

        // True attempts strips kneels from YPA denominator.
        int trueAttempts = att - kd;

        return new RushingStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            AvgYards                    = stats.AvgYards,
            Attempts                    = stats.Attempts,
            Touchdowns                  = stats.Touchdowns,
            TacklesForLoss              = stats.TacklesForLoss,
            TacklesForLossYards         = stats.TacklesForLossYards,
            Yards                       = stats.Yards,
            Longest                     = stats.Longest,
            LongestTouchdown            = stats.LongestTouchdown,
            RedzoneAttempts             = stats.RedzoneAttempts,
            BrokenTackles               = stats.BrokenTackles,
            KneelDowns                  = stats.KneelDowns,
            Scrambles                   = stats.Scrambles,
            YardsAfterContact           = stats.YardsAfterContact,
            FirstDowns                  = stats.FirstDowns,

            // ── Efficiency ────────────────────────────────────────────────
            YardsPerCarry               = Calc.Round(Calc.Ratio(yds, att)),
            YardsAfterContactPerCarry   = Calc.Round(Calc.Ratio(stats.YardsAfterContact, att)),
            BrokenTackleRate            = Calc.Round(Calc.Ratio(stats.BrokenTackles, att), 3),
            TdRate                      = Calc.Round(Calc.Pct(stats.Touchdowns, att)),
            FirstDownRate               = Calc.Round(Calc.Pct(stats.FirstDowns ?? 0, att)),

            // ── Negative Plays ────────────────────────────────────────────
            TflRate                     = Calc.Round(Calc.Pct((double)stats.TacklesForLoss, att)),
            TflYardsPerTfl              = Calc.Round(Calc.Ratio(stats.TacklesForLossYards, (double)stats.TacklesForLoss)),

            // ── Redzone ───────────────────────────────────────────────────
            RedzoneAttemptRate          = Calc.Round(Calc.Pct(stats.RedzoneAttempts, att)),
            RedzoneTdConversionRate     = Calc.Round(Calc.Pct(stats.Touchdowns, stats.RedzoneAttempts)),

            // ── QB-Specific ───────────────────────────────────────────────
            ScrambleRate                = Calc.Round(Calc.Pct(stats.Scrambles, att)),
            KneelDownRate               = Calc.Round(Calc.Pct(kd, att)),
            AdjustedYardsPerCarry       = Calc.Round(Calc.Ratio(yds, trueAttempts)),
        };
    }
}

/// <summary>
/// Calculates all derived receiving metrics from a <see cref="SeasonReceivingStats"/> block.
/// </summary>
public static class ReceivingCalculator
{
    /// <summary>
    /// Calculates all derived receiving metrics. Returns <c>null</c> if <paramref name="stats"/> is <c>null</c>.
    /// </summary>
    public static ReceivingStatsResult? Calculate(SeasonReceivingStats? stats)
    {
        if (stats is null) return null;

        int targets  = stats.Targets;
        int recs     = stats.Receptions;
        int yds      = stats.Yards;
        int catchable = stats.CatchablePasses;

        return new ReceivingStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Targets                        = stats.Targets,
            Receptions                     = stats.Receptions,
            AvgYards                       = stats.AvgYards,
            Yards                          = stats.Yards,
            Touchdowns                     = stats.Touchdowns,
            YardsAfterCatch                = stats.YardsAfterCatch,
            Longest                        = stats.Longest,
            LongestTouchdown               = stats.LongestTouchdown,
            RedzoneTargets                 = stats.RedzoneTargets,
            AirYards                       = stats.AirYards,
            BrokenTackles                  = stats.BrokenTackles,
            DroppedPasses                  = stats.DroppedPasses,
            CatchablePasses                = stats.CatchablePasses,
            YardsAfterContact              = stats.YardsAfterContact,
            FirstDowns                     = stats.FirstDowns,

            // ── Volume / Opportunity ──────────────────────────────────────
            CatchRate                      = Calc.Round(Calc.Pct(recs, targets)),
            TrueCatchRate                  = Calc.Round(Calc.Pct(recs, catchable)),
            RedzoneTargetRate              = Calc.Round(Calc.Pct(stats.RedzoneTargets, targets)),
            UncatchablePassRate            = Calc.Round(Calc.Pct(targets - catchable, targets)),

            // ── Efficiency ────────────────────────────────────────────────
            YardsPerTarget                 = Calc.Round(Calc.Ratio(yds, targets)),
            YardsPerReception              = Calc.Round(Calc.Ratio(yds, recs)),
            AverageDepthOfTarget           = Calc.Round(Calc.Ratio(stats.AirYards, targets)),
            AirYardsPerReception           = Calc.Round(Calc.Ratio(stats.AirYards, recs)),
            YacPerReception                = Calc.Round(Calc.Ratio(stats.YardsAfterCatch, recs)),
            YacShareOfTotalYards           = Calc.Round(Calc.Pct(stats.YardsAfterCatch, yds)),
            YardsAfterContactPerReception  = Calc.Round(Calc.Ratio(stats.YardsAfterContact, recs)),
            BrokenTackleRate               = Calc.Round(Calc.Ratio(stats.BrokenTackles, recs), 3),
            TdRate                         = Calc.Round(Calc.Pct(stats.Touchdowns, recs)),
            FirstDownRate                  = Calc.Round(Calc.Pct(stats.FirstDowns ?? 0, recs)),

            // ── Ball Security ─────────────────────────────────────────────
            DropRate                       = Calc.Round(Calc.Pct(stats.DroppedPasses, catchable)),
        };
    }
}

/// <summary>
/// Calculates all derived defensive metrics from <see cref="SeasonDefenseStats"/> and,
/// optionally, <see cref="SeasonIntReturnStats"/>.
/// </summary>
public static class DefenseCalculator
{
    /// <summary>
    /// Calculates all derived defensive metrics.
    /// Returns <c>null</c> if <paramref name="stats"/> is <c>null</c>.
    /// </summary>
    public static DefenseStatsResult? Calculate(SeasonDefenseStats? stats, SeasonIntReturnStats? intReturns = null)
    {
        if (stats is null) return null;

        int combined     = stats.Combined;
        int blitzes      = stats.Blitzes;
        int defTargets   = stats.DefTargets;

        return new DefenseStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Tackles                 = stats.Tackles,
            Assists                 = stats.Assists,
            Combined                = stats.Combined,
            Sacks                   = stats.Sacks,
            SackYards               = stats.SackYards,
            Interceptions           = stats.Interceptions,
            PassesDefended          = stats.PassesDefended,
            ForcedFumbles           = stats.ForcedFumbles,
            FumbleRecoveries        = stats.FumbleRecoveries,
            QbHits                  = stats.QbHits,
            TacklesForLoss          = stats.Tloss,
            TacklesForLossYards     = stats.TlossYards,
            SpTackles               = stats.SpTackles,
            SpAssists               = stats.SpAssists,
            SpForcedFumbles         = stats.SpForcedFumbles,
            SpFumbleRecoveries      = stats.SpFumbleRecoveries,
            MiscTackles             = stats.MiscTackles,
            MiscAssists             = stats.MiscAssists,
            MiscForcedFumbles       = stats.MiscForcedFumbles,
            MiscFumbleRecoveries    = stats.MiscFumbleRecoveries,
            SpOwnFumbleRecoveries   = stats.SpOwnFumbleRecoveries,
            SpOppFumbleRecoveries   = stats.SpOppFumbleRecoveries,
            DefTargets              = stats.DefTargets,
            DefComps                = stats.DefComps,
            Blitzes                 = stats.Blitzes,
            Hurries                 = stats.Hurries,
            Knockdowns              = stats.Knockdowns,
            MissedTackles           = stats.MissedTackles,
            BattedPasses            = stats.BattedPasses,

            // ── Tackles & Stops ───────────────────────────────────────────
            SoloTackleRate          = Calc.Round(Calc.Pct(stats.Tackles, combined)),
            MissedTackleRate        = Calc.Round(Calc.Pct(stats.MissedTackles, combined + stats.MissedTackles)),
            TflRate                 = Calc.Round(Calc.Pct((double)stats.Tloss, combined)),
            TflYardsPerTfl          = Calc.Round(Calc.Ratio((double)stats.TlossYards, (double)stats.Tloss)),
            SackRate                = Calc.Round(Calc.Pct((double)stats.Sacks, combined)),
            SackYardsPerSack        = Calc.Round(Calc.Ratio((double)stats.SackYards, (double)stats.Sacks)),
            Safeties                = stats.Safeties,
            FourthDownStops         = stats.FourthDownStops,
            ThreeAndOutsForced      = stats.ThreeAndOutsForced,

            // ── Pass Rush ─────────────────────────────────────────────────
            QbHitRateOnBlitz        = Calc.Round(Calc.Pct(stats.QbHits, blitzes)),
            HurryRateOnBlitz        = Calc.Round(Calc.Pct(stats.Hurries, blitzes)),
            KnockdownRateOnBlitz    = Calc.Round(Calc.Pct(stats.Knockdowns, blitzes)),
            BattedPassRateOnBlitz   = Calc.Round(Calc.Pct(stats.BattedPasses, blitzes)),
            TotalPressureRate       = Calc.Round(Calc.Pct(
                                          (double)stats.Sacks + stats.QbHits + stats.Hurries, blitzes)),

            // ── Coverage ──────────────────────────────────────────────────
            CompletionPctAllowed    = Calc.Round(Calc.Pct(stats.DefComps, defTargets)),
            PassDefenseRate         = Calc.Round(Calc.Pct(stats.PassesDefended, defTargets)),
            InterceptionRateOnTargets = Calc.Round(Calc.Pct(stats.Interceptions, defTargets)),
            CompletionAvoidedRate   = Calc.Round(
                                          Calc.Pct(defTargets - stats.DefComps - stats.Interceptions, defTargets)),

            // ── Turnovers ─────────────────────────────────────────────────
            ForcedFumbleRate        = Calc.Round(Calc.Pct(stats.ForcedFumbles, combined)),
            FumbleRecoveryRate      = Calc.Round(
                                          Calc.Ratio(stats.FumbleRecoveries,
                                              stats.ForcedFumbles + stats.MiscForcedFumbles)),

            // ── INT Returns ───────────────────────────────────────────────
            IntReturnAverage        = intReturns is not null
                                          ? Calc.Round(Calc.Ratio(intReturns.Yards, intReturns.Returns))
                                          : 0.0,
            PickSixRate             = intReturns is not null
                                          ? Calc.Round(Calc.Pct(intReturns.Touchdowns, intReturns.Returns))
                                          : 0.0,

            // ── Special Teams ─────────────────────────────────────────────
            SpSoloTackleRate        = Calc.Round(Calc.Pct(stats.SpTackles, stats.SpTackles + stats.SpAssists)),
            SpForcedFumbleRate      = Calc.Round(Calc.Pct(stats.SpForcedFumbles, stats.SpTackles + stats.SpAssists)),
            SpBlocks                = stats.SpBlocks,
        };
    }
}
