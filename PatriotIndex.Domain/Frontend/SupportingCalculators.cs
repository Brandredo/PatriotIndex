using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Calculates derived fumble metrics from a <see cref="SeasonFumbleStats"/> block.
/// </summary>
public static class FumbleCalculator
{
    /// <summary>
    /// Calculates derived fumble metrics.
    /// Pass <paramref name="totalTouches"/> (rushes + receptions) to include the per-touch rate.
    /// </summary>
    public static FumbleStatsResult? Calculate(SeasonFumbleStats? stats, int? totalTouches = null)
    {
        if (stats is null) return null;

        return new FumbleStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Fumbles         = stats.Fumbles,
            LostFumbles     = stats.LostFumbles,
            OwnRec          = stats.OwnRec,
            OwnRecYards     = stats.OwnRecYards,
            OppRec          = stats.OppRec,
            OppRecYards     = stats.OppRecYards,
            OutOfBounds     = stats.OutOfBounds,
            OwnRecTds       = stats.OwnRecTds,
            OppRecTds       = stats.OppRecTds,
            EzRecTds        = stats.EzRecTds,

            LostFumbleRate         = Calc.Round(Calc.Pct(stats.LostFumbles, stats.Fumbles)),
            FumbleRatePerTouch     = totalTouches.HasValue && totalTouches.Value > 0
                                         ? Calc.Round(Calc.Pct(stats.Fumbles, totalTouches.Value))
                                         : null,
            TotalFumbleRecoveryTds = stats.OwnRecTds + stats.OppRecTds + stats.EzRecTds,
            ForcedFumbles          = stats.ForcedFumbles,
        };
    }
}

/// <summary>
/// Calculates derived penalty metrics from a <see cref="SeasonPenaltyStats"/> block.
/// </summary>
public static class PenaltyCalculator
{
    public static PenaltyStatsResult? Calculate(SeasonPenaltyStats? stats)
    {
        if (stats is null) return null;

        return new PenaltyStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Penalties   = stats.Penalties,
            Yards       = stats.Yards,
            FirstDowns  = stats.FirstDowns,

            YardsPerPenalty = Calc.Round(Calc.Ratio(stats.Yards, stats.Penalties)),
            FirstDownRate   = stats.FirstDowns.HasValue
                                  ? Calc.Round(Calc.Pct(stats.FirstDowns.Value, stats.Penalties))
                                  : null,
        };
    }
}

/// <summary>
/// Calculates derived extra-point metrics for a player (kicker) from <see cref="SeasonPlayerExtraPointStats"/>.
/// </summary>
public static class PlayerExtraPointCalculator
{
    public static PlayerExtraPointStatsResult? Calculate(SeasonPlayerExtraPointStats? stats)
    {
        if (stats is null) return null;

        return new PlayerExtraPointStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Attempts    = stats.Attempts,
            Made        = stats.Made,
            Blocked     = stats.Blocked,
            Missed      = stats.Missed,
            Pct         = stats.Pct,

            XpPct      = Calc.Round(Calc.Pct(stats.Made, stats.Attempts)),
            BlockRate  = Calc.Round(Calc.Pct(stats.Blocked, stats.Attempts)),
            MissRate   = Calc.Round(Calc.Pct(stats.Missed, stats.Attempts)),
        };
    }
}

/// <summary>
/// Calculates derived team-level extra-point and two-point conversion metrics
/// from a <see cref="SeasonTeamExtraPointStats"/> block.
/// </summary>
public static class TeamExtraPointCalculator
{
    public static TeamExtraPointStatsResult? Calculate(SeasonTeamExtraPointStats? stats)
    {
        if (stats is null) return null;

        var kicks = stats.Kicks;
        var conv  = stats.Conversions;

        int totalOffenseAttempts  = (conv?.PassAttempts ?? 0) + (conv?.RushAttempts ?? 0);
        int totalOffenseSuccesses = (conv?.PassSuccesses ?? 0) + (conv?.RushSuccesses ?? 0);

        return new TeamExtraPointStatsResult
        {
            XpKickPct               = kicks is not null ? Calc.Round(Calc.Pct(kicks.Made, kicks.Attempts))      : 0.0,
            XpKickBlockRate         = kicks is not null ? Calc.Round(Calc.Pct(kicks.Blocked, kicks.Attempts))   : 0.0,
            TwoPointPassConvPct     = conv  is not null ? Calc.Round(Calc.Pct(conv.PassSuccesses, conv.PassAttempts))     : 0.0,
            TwoPointRushConvPct     = conv  is not null ? Calc.Round(Calc.Pct(conv.RushSuccesses, conv.RushAttempts))     : 0.0,
            TwoPointDefenseConvPct  = conv  is not null ? Calc.Round(Calc.Pct(conv.DefenseSuccesses, conv.DefenseAttempts)) : 0.0,
            OverallTwoPointConvPct  = Calc.Round(Calc.Pct(totalOffenseSuccesses, totalOffenseAttempts)),
            TwoPointTurnoverSuccesses = conv?.TurnoverSuccesses ?? 0,
        };
    }
}

/// <summary>
/// Calculates derived situational efficiency metrics from a <see cref="SeasonEfficiencyStats"/> block.
/// Team-level only.
/// </summary>
public static class EfficiencyCalculator
{
    public static EfficiencyStatsResult? Calculate(SeasonEfficiencyStats? stats)
    {
        if (stats is null) return null;

        return new EfficiencyStatsResult
        {
            ThirdDownConvPct    = stats.Thirddown  is not null ? Calc.Round((double)stats.Thirddown.Pct)  : 0.0,
            FourthDownConvPct   = stats.Fourthdown is not null ? Calc.Round((double)stats.Fourthdown.Pct) : 0.0,
            RedzoneConvPct      = stats.Redzone    is not null ? Calc.Round((double)stats.Redzone.Pct)    : 0.0,
            GoalToGoConvPct     = stats.Goaltogo   is not null ? Calc.Round((double)stats.Goaltogo.Pct)   : 0.0,

            ThirdDownAttempts   = stats.Thirddown?.Attempts  ?? 0,
            FourthDownAttempts  = stats.Fourthdown?.Attempts ?? 0,
            RedzoneAttempts     = stats.Redzone?.Attempts    ?? 0,
            GoalToGoAttempts    = stats.Goaltogo?.Attempts   ?? 0,
        };
    }
}

/// <summary>
/// Calculates derived first-down distribution metrics from a <see cref="SeasonFirstDownStats"/> block.
/// Team-level only.
/// </summary>
public static class FirstDownCalculator
{
    public static FirstDownStatsResult? Calculate(SeasonFirstDownStats? stats)
    {
        if (stats is null) return null;

        int total = stats.Total;

        return new FirstDownStatsResult
        {
            PassFirstDownPct    = Calc.Round(Calc.Pct(stats.Pass, total)),
            RushFirstDownPct    = Calc.Round(Calc.Pct(stats.Rush, total)),
            PenaltyFirstDownPct = Calc.Round(Calc.Pct(stats.Penalty, total)),
            Total               = total,
        };
    }
}

/// <summary>
/// Calculates derived touchdown distribution metrics from a <see cref="SeasonTouchdownStats"/> block.
/// Team-level only.
/// </summary>
public static class TouchdownCalculator
{
    public static TouchdownDistributionResult? Calculate(SeasonTouchdownStats? stats)
    {
        if (stats is null) return null;

        int total = stats.Total;

        return new TouchdownDistributionResult
        {
            PassTdPct       = Calc.Round(Calc.Pct(stats.Pass, total)),
            RushTdPct       = Calc.Round(Calc.Pct(stats.Rush, total)),
            ReturnTdPct     = Calc.Round(Calc.Pct(stats.TotalReturn, total)),
            DefensiveTdPct  = Calc.Round(Calc.Pct(stats.FumbleReturn + stats.IntReturn, total)),
            KickReturnTdPct = Calc.Round(Calc.Pct(stats.KickReturn, total)),
            PuntReturnTdPct = Calc.Round(Calc.Pct(stats.PuntReturn, total)),
            Total           = total,
        };
    }
}

/// <summary>
/// Calculates derived team interception metrics from a <see cref="SeasonInterceptionStats"/> block.
/// Team-level only.
/// </summary>
public static class TeamInterceptionCalculator
{
    public static TeamInterceptionStatsResult? Calculate(SeasonInterceptionStats? stats)
    {
        if (stats is null) return null;

        return new TeamInterceptionStatsResult
        {
            ReturnRate         = Calc.Round(Calc.Pct(stats.Returned, stats.Interceptions)),
            AverageReturnYards = Calc.Round(Calc.Ratio(stats.ReturnYards, stats.Returned)),
            Interceptions      = stats.Interceptions,
        };
    }
}
