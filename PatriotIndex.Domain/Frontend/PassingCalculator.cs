using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Calculates all derived passing metrics from a <see cref="SeasonPassingStats"/> block.
/// </summary>
public static class PassingCalculator
{
    // Bracket weights used by the difficulty-weighted FG% (not applicable here, kept for parity pattern).
    // NFL passer rating component ceiling.
    private const double RatingComponentMax = 2.375;

    /// <summary>
    /// Calculates all derived passing metrics. Returns <c>null</c> if <paramref name="stats"/> is <c>null</c>.
    /// </summary>
    public static PassingStatsResult? Calculate(SeasonPassingStats? stats)
    {
        if (stats is null) return null;

        int att = stats.Attempts;
        int cmp = stats.Completions;
        int yds = stats.Yards;
        int td  = stats.Touchdowns;
        int ints = stats.Interceptions;
        int sacks = stats.Sacks;

        // Adjusted attempt denominator (attempts + sacks) used in net/adjusted metrics.
        int trueDropbacks = att + sacks;

        // Meaningful attempts: strip intentional incompletions for on-target rate.
        int meaningfulAttempts = att - stats.ThrowAways - stats.Spikes;

        return new PassingStatsResult
        {
            
            Attempts = stats.Attempts,
            Completions = stats.Completions,
            Interceptions = stats.Interceptions,
            SackYards = stats.SackYards,
            Rating = stats.Rating,
            Touchdowns = stats.Touchdowns,
            AvgYards = stats.AvgYards,
            Sacks = stats.Sacks,
            Yards = stats.Yards,
            IntTouchdowns = stats.IntTouchdowns,
            Longest = stats.Longest,
            LongestTouchdown = stats.LongestTouchdown,
            AirYards = stats.AirYards,
            RedzoneAttempts = stats.RedzoneAttempts,
            NetYards = stats.NetYards,
            ThrowAways = stats.ThrowAways,
            Spikes = stats.Spikes,
            DroppedPasses = stats.DroppedPasses,
            DefendedPasses = stats.DefendedPasses,
            BattedPasses = stats.BattedPasses,
            OnTargetThrows = stats.OnTargetThrows,
            PoorThrows = stats.PoorThrows,
            Blitzes = stats.Blitzes,
            Hurries = stats.Hurries,
            Knockdowns = stats.Knockdowns,
            PocketTime = stats.PocketTime,
            GrossYards = stats.GrossYards,
            FirstDowns = stats.FirstDowns,
            AvgPocketTime = stats.AvgPocketTime,
            
            
            // ── Basic Ratios ──────────────────────────────────────────────
            CompletionPct              = Calc.Round(Calc.Pct(cmp, att)),
            YardsPerAttempt            = Calc.Round(Calc.Ratio(yds, att)),
            TdPct                      = Calc.Round(Calc.Pct(td, att)),
            IntPct                     = Calc.Round(Calc.Pct(ints, att)),
            TdIntRatio                 = Calc.Round(Calc.Ratio(td, ints)),
            SackRate                   = Calc.Round(Calc.Pct(sacks, trueDropbacks)),
            SackYardsPerSack           = Calc.Round(Calc.Ratio(stats.SackYards, sacks)),
            GrossNetYardsDelta         = stats.GrossYards - stats.NetYards,
            ThrowAwayRate              = Calc.Round(Calc.Pct(stats.ThrowAways, att)),
            SpikeRate                  = Calc.Round(Calc.Pct(stats.Spikes, att)),
            BattedPassRate             = Calc.Round(Calc.Pct(stats.BattedPasses, att)),
            DefendedPassRate           = Calc.Round(Calc.Pct(stats.DefendedPasses, att)),
            KnockdownRate              = Calc.Round(Calc.Pct(stats.Knockdowns, att)),
            BlitzRate                  = Calc.Round(Calc.Pct(stats.Blitzes, att)),
            FirstDownPassRate          = Calc.Round(Calc.Pct(stats.FirstDowns ?? 0, cmp)),
            AveragePocketTime          = stats.AvgPocketTime.HasValue
                                            ? Calc.Round(stats.AvgPocketTime.Value, 2)
                                            : Calc.Round(Calc.Ratio(stats.PocketTime, att), 2),

            // ── Advanced / Composite ──────────────────────────────────────
            PasserRating               = Calc.Round(NflPasserRating(att, cmp, yds, td, ints), 1),
            NetYardsPerAttempt         = Calc.Round(Calc.Ratio(yds - stats.SackYards, trueDropbacks)),
            AdjustedNetYardsPerAttempt = Calc.Round(AnyA(yds, td, ints, stats.SackYards, att, sacks)),
            OnTargetThrowRate          = Calc.Round(Calc.Pct(stats.OnTargetThrows, meaningfulAttempts)),
            PoorThrowRate              = Calc.Round(Calc.Pct(stats.PoorThrows, att)),
            DropRate                   = Calc.Round(Calc.Pct(stats.DroppedPasses, att)),
            DropAdjustedCompletionPct  = Calc.Round(Calc.Pct(cmp + stats.DroppedPasses, att)),
            HurryRateWhenBlitzed       = Calc.Round(Calc.Pct(stats.Hurries, stats.Blitzes)),

            // ── Air Yards / Depth ─────────────────────────────────────────
            AverageDepthOfTarget       = Calc.Round(Calc.Ratio(stats.AirYards, att)),
            AirYardsPerCompletion      = Calc.Round(Calc.Ratio(stats.AirYards, cmp)),
            YacPerCompletion           = Calc.Round(Calc.Ratio(yds - stats.AirYards, cmp)),
            RedzoneAttemptRate         = Calc.Round(Calc.Pct(stats.RedzoneAttempts, att)),
            //RedzoneTouchdownRate       = Calc.Round(Calc.Pct(stats.RedzoneTouchdowns, stats.RedzoneAttempts)),
            //RedzoneCompletionPct       = Calc.Round(Calc.Pct(stats.RedzoneCompletions, stats.RedzoneAttempts)),
        };
    }

    // ── Private formulae ─────────────────────────────────────────────────────

    /// <summary>
    /// Computes the official NFL passer rating (0–158.3).
    /// </summary>
    private static double NflPasserRating(int attempts, int completions, int yards, int touchdowns, int interceptions)
    {
        if (attempts == 0) return 0.0;

        double a = Calc.Clamp(((double)completions / attempts - 0.3) * 5.0, 0, RatingComponentMax);
        double b = Calc.Clamp(((double)yards      / attempts - 3.0) * 0.25, 0, RatingComponentMax);
        double c = Calc.Clamp(((double)touchdowns / attempts) * 20.0,        0, RatingComponentMax);
        double d = Calc.Clamp(RatingComponentMax - ((double)interceptions / attempts * 25.0), 0, RatingComponentMax);

        return (a + b + c + d) / 6.0 * 100.0;
    }

    /// <summary>
    /// Adjusted Net Yards per Attempt (ANY/A):
    /// (Yards + 20×TD − 45×INT − SackYards) / (Attempts + Sacks)
    /// </summary>
    private static double AnyA(int yards, int td, int ints, int sackYards, int attempts, int sacks)
    {
        int denominator = attempts + sacks;
        if (denominator == 0) return 0.0;

        double numerator = yards + (20 * td) - (45 * ints) - sackYards;
        return numerator / denominator;
    }
}
