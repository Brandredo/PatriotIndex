using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <inheritdoc cref="ITeamStatsService"/>
public sealed class TeamStatsService : ITeamStatsService
{
    /// <inheritdoc/>
    public TeamStatsResult Calculate(TeamStatBlock stats)
    {
        ArgumentNullException.ThrowIfNull(stats);

        // ── Per-category calculations ─────────────────────────────────────────

        var passing    = PassingCalculator.Calculate(stats.Passing);
        var rushing    = RushingCalculator.Calculate(stats.Rushing);
        var receiving  = ReceivingCalculator.Calculate(stats.Receiving);
        var defense    = DefenseCalculator.Calculate(stats.Defense, stats.IntReturns);
        var fieldGoals = FieldGoalCalculator.Calculate(stats.FieldGoals);
        var kickoffs   = KickoffCalculator.Calculate(stats.Kickoffs);
        var punts      = PuntingCalculator.Calculate(stats.Punts);
        var kickRet    = KickReturnCalculator.Calculate(stats.KickReturns);
        var puntRet    = PuntReturnCalculator.Calculate(stats.PuntReturns);
        var intRet     = IntReturnCalculator.Calculate(stats.IntReturns);
        var miscRet    = MiscReturnCalculator.Calculate(stats.MiscReturns);
        var fumbles    = FumbleCalculator.Calculate(stats.Fumbles);
        var penalties  = PenaltyCalculator.Calculate(stats.Penalties);
        var extraPts   = TeamExtraPointCalculator.Calculate(stats.ExtraPoints);

        // ── Team-only calculations ────────────────────────────────────────────

        var efficiency  = EfficiencyCalculator.Calculate(stats.Efficiency);
        var firstDowns  = FirstDownCalculator.Calculate(stats.FirstDowns);
        var tdDist      = TouchdownCalculator.Calculate(stats.Touchdowns);
        var intercepts  = TeamInterceptionCalculator.Calculate(stats.Interceptions);

        // ── Cross-block derived metrics ───────────────────────────────────────

        var turnoverDiff  = CalculateTurnoverDifferential(stats);
        var redzoneTdRate = CalculateRedzoneTdRate(stats);

        return new TeamStatsResult
        {
            // Offense
            Passing               = passing,
            Rushing               = rushing,
            Receiving             = receiving,

            // Defense
            Defense               = defense,
            Interceptions         = intercepts,

            // Special Teams
            FieldGoals            = fieldGoals,
            Kickoffs              = kickoffs,
            Punts                 = punts,
            KickReturns           = kickRet,
            PuntReturns           = puntRet,
            IntReturns            = intRet,
            MiscReturns           = miscRet,
            ExtraPoints           = extraPts,

            // Turnovers / Penalties
            Fumbles               = fumbles,
            Penalties             = penalties,

            // Team-Only
            Efficiency            = efficiency,
            FirstDowns            = firstDowns,
            TouchdownDistribution = tdDist,
            TurnoverDifferential  = turnoverDiff,

            // Cross-block
            RedzoneTdRate         = redzoneTdRate,
        };
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    /// <summary>
    /// Builds a turnover differential using defense takeaways vs. offensive giveaways.
    /// Requires Defense and at least one of Passing/Fumbles to be non-null.
    /// </summary>
    private static TurnoverDifferentialResult? CalculateTurnoverDifferential(TeamStatBlock stats)
    {
        if (stats.Defense is null) return null;

        int takeaways = stats.Defense.Interceptions + stats.Defense.FumbleRecoveries;
        int giveaways = (stats.Passing?.Interceptions ?? 0) + (stats.Fumbles?.LostFumbles ?? 0);

        return new TurnoverDifferentialResult
        {
            Takeaways    = takeaways,
            Giveaways    = giveaways,
            Differential = takeaways - giveaways,
        };
    }

    /// <summary>
    /// Calculates the team red-zone touchdown rate:
    /// (PassingTDs + RushingTDs) / Redzone.Attempts × 100.
    /// Null if efficiency or both passing/rushing blocks are unavailable.
    /// </summary>
    private static double? CalculateRedzoneTdRate(TeamStatBlock stats)
    {
        int redzoneAttempts = stats.Efficiency?.Redzone?.Attempts ?? 0;
        if (redzoneAttempts == 0) return null;

        int tds = (stats.Passing?.Touchdowns ?? 0) + (stats.Rushing?.Touchdowns ?? 0);
        return Calc.Round(Calc.Pct(tds, redzoneAttempts));
    }
}
