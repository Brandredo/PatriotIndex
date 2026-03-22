using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <inheritdoc cref="IPlayerStatsService"/>
public sealed class PlayerStatsService : IPlayerStatsService
{
    /// <inheritdoc/>
    public PlayerStatsResult Calculate(PlayerStatBlock stats)
    {
        ArgumentNullException.ThrowIfNull(stats);

        // ── Per-category calculations ─────────────────────────────────────────

        var passing    = PassingCalculator.Calculate(stats.Passing);
        var rushing    = RushingCalculator.Calculate(stats.Rushing);
        var receiving  = ReceivingCalculator.Calculate(stats.Receiving);
        var defense    = DefenseCalculator.Calculate(stats.Defense, stats.IntReturns);
        var fieldGoals = FieldGoalCalculator.Calculate(stats.FieldGoals);
        var xp         = PlayerExtraPointCalculator.Calculate(stats.ExtraPoints);
        var kickoffs   = KickoffCalculator.Calculate(stats.Kickoffs);
        var punts      = PuntingCalculator.Calculate(stats.Punts);
        var kickRet    = KickReturnCalculator.Calculate(stats.KickReturns);
        var puntRet    = PuntReturnCalculator.Calculate(stats.PuntReturns);
        var intRet     = IntReturnCalculator.Calculate(stats.IntReturns);
        var miscRet    = MiscReturnCalculator.Calculate(stats.MiscReturns);
        var penalties  = PenaltyCalculator.Calculate(stats.Penalties);

        // ── Touch total for fumble rate (rushing attempts + receptions) ────────
        int? totalTouches = Resolvetouches(stats);
        var fumbles = FumbleCalculator.Calculate(stats.Fumbles, totalTouches);

        // ── Cross-block derived metrics ───────────────────────────────────────
        var (scrimmageYards, offTds, turnoverRate, fumbleRatePerTouch) =
            CalculateCrossBlockMetrics(stats, totalTouches);

        return new PlayerStatsResult
        {
            Passing               = passing,
            Rushing               = rushing,
            Receiving             = receiving,
            Defense               = defense, 
            FieldGoals            = fieldGoals,
            ExtraPoints           = xp,
            Kickoffs              = kickoffs,
            Punts                 = punts,
            KickReturns           = kickRet,
            PuntReturns           = puntRet,
            IntReturns            = intRet,
            MiscReturns           = miscRet,
            Fumbles               = fumbles,
            Penalties             = penalties,

            // Cross-block
            ScrimmageYards          = scrimmageYards,
            TotalTouches            = totalTouches,
            TotalOffensiveTouchdowns = offTds,
            TotalTurnoverRate       = turnoverRate,
            FumbleRatePerTouch      = fumbleRatePerTouch,
        };
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    /// <summary>
    /// Returns total touches (rushing attempts + receiving receptions) when both blocks exist.
    /// </summary>
    private static int? Resolvetouches(PlayerStatBlock stats)
    {
        bool hasRush = stats.Rushing is not null;
        bool hasRec  = stats.Receiving is not null;
        if (!hasRush && !hasRec) return null;

        return (stats.Rushing?.Attempts ?? 0) + (stats.Receiving?.Receptions ?? 0);
    }

    /// <summary>
    /// Computes cross-block composite metrics that require more than one stat block.
    /// Returns a tuple of (scrimmageYards, offTouchdowns, turnoverRate, fumbleRatePerTouch).
    /// </summary>
    private static (int? scrimmageYards, int? offTds, double? turnoverRate, double? fumbleRatePerTouch)
        CalculateCrossBlockMetrics(PlayerStatBlock stats, int? totalTouches)
    {
        // ── Scrimmage yards (RB / WR / TE) ────────────────────────────────────
        int? scrimmageYards = null;
        if (stats.Rushing is not null || stats.Receiving is not null)
        {
            scrimmageYards = (stats.Rushing?.Yards ?? 0) + (stats.Receiving?.Yards ?? 0);
        }

        // ── Total offensive touchdowns ─────────────────────────────────────────
        int? offTds = null;
        if (stats.Rushing is not null || stats.Receiving is not null)
        {
            offTds = (stats.Rushing?.Touchdowns ?? 0) + (stats.Receiving?.Touchdowns ?? 0);
        }

        // ── Turnover rate (QB: INTs + lost fumbles per play) ───────────────────
        double? turnoverRate = null;
        if (stats.Passing is not null && stats.Fumbles is not null)
        {
            int plays = (stats.Passing.Attempts) + (stats.Rushing?.Attempts ?? 0);
            if (plays > 0)
            {
                double turnovers = stats.Passing.Interceptions + stats.Fumbles.LostFumbles;
                turnoverRate = Calc.Round(Calc.Pct(turnovers, plays));
            }
        }

        // ── Fumble rate per touch ─────────────────────────────────────────────
        double? fumbleRatePerTouch = null;
        if (stats.Fumbles is not null && totalTouches.HasValue && totalTouches.Value > 0)
        {
            fumbleRatePerTouch = Calc.Round(Calc.Pct(stats.Fumbles.Fumbles, totalTouches.Value));
        }

        return (scrimmageYards, offTds, turnoverRate, fumbleRatePerTouch);
    }
}
