using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Calculates all derived field-goal metrics from a <see cref="SeasonFieldGoalStats"/> block.
/// </summary>
public static class FieldGoalCalculator
{
    // ── League-average FG% by distance bracket (used for Points Above Expected) ──
    // Based on historical NFL averages. Replace with season-specific constants if desired.
    private static readonly Dictionary<string, double> LeagueAvgFgPct = new()
    {
        ["1-19"]  = 0.990,
        ["20-29"] = 0.955,
        ["30-39"] = 0.900,
        ["40-49"] = 0.800,
        ["50+"]   = 0.620,
    };

    // Difficulty weights applied to each bracket for the weighted FG% metric.
    private static readonly Dictionary<string, double> BracketWeights = new()
    {
        ["1-19"]  = 1.0,
        ["20-29"] = 1.2,
        ["30-39"] = 1.5,
        ["40-49"] = 1.8,
        ["50+"]   = 2.2,
    };

    /// <summary>
    /// Calculates all derived field-goal metrics. Returns <c>null</c> if <paramref name="stats"/> is <c>null</c>.
    /// </summary>
    public static FieldGoalStatsResult? Calculate(SeasonFieldGoalStats? stats)
    {
        if (stats is null) return null;

        return new FieldGoalStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Attempts    = stats.Attempts,
            Made        = stats.Made,
            Blocked     = stats.Blocked,
            Yards       = stats.Yards,
            AvgYards    = stats.AvgYards,
            Longest     = stats.Longest,
            Missed      = stats.Missed,
            Pct         = stats.Pct,
            Attempts19  = stats.Attempts19,
            Attempts29  = stats.Attempts29,
            Attempts39  = stats.Attempts39,
            Attempts49  = stats.Attempts49,
            Attempts50  = stats.Attempts50,
            Made19      = stats.Made19,
            Made29      = stats.Made29,
            Made39      = stats.Made39,
            Made49      = stats.Made49,
            Made50      = stats.Made50,

            OverallFgPct            = Calc.Round(Calc.Pct(stats.Made, stats.Attempts)),
            MissRate                = Calc.Round(Calc.Pct(stats.Missed, stats.Attempts)),
            BlockRate               = Calc.Round(Calc.Pct(stats.Blocked, stats.Attempts)),
            AverageAttemptDistance  = Calc.Round(Calc.Ratio(stats.Yards, stats.Attempts)),

            FgPct1To19              = Calc.Round(Calc.Pct(stats.Made19, stats.Attempts19)),
            FgPct20To29             = Calc.Round(Calc.Pct(stats.Made29, stats.Attempts29)),
            FgPct30To39             = Calc.Round(Calc.Pct(stats.Made39, stats.Attempts39)),
            FgPct40To49             = Calc.Round(Calc.Pct(stats.Made49, stats.Attempts49)),
            FgPct50Plus             = Calc.Round(Calc.Pct(stats.Made50, stats.Attempts50)),

            PointsAboveExpected     = Calc.Round(CalculatePae(stats)),
            DifficultyWeightedFgPct = Calc.Round(CalculateWeightedFgPct(stats)),
        };
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    /// <summary>
    /// Points Above Expected: for each bracket, (made − league_avg × attempts) × 3.
    /// Rewards kickers who outperform league average; penalises those who underperform.
    /// </summary>
    private static double CalculatePae(SeasonFieldGoalStats s)
    {
        double pae = 0.0;
        pae += (s.Made19  - LeagueAvgFgPct["1-19"]  * s.Attempts19)  * 3;
        pae += (s.Made29  - LeagueAvgFgPct["20-29"] * s.Attempts29)  * 3;
        pae += (s.Made39  - LeagueAvgFgPct["30-39"] * s.Attempts39)  * 3;
        pae += (s.Made49  - LeagueAvgFgPct["40-49"] * s.Attempts49)  * 3;
        pae += (s.Made50  - LeagueAvgFgPct["50+"]   * s.Attempts50)  * 3;
        return pae;
    }

    /// <summary>
    /// Difficulty-weighted FG%: Σ(made × weight) / Σ(attempts × weight).
    /// Brackets with harder distances receive higher weights so distance composition
    /// does not artificially inflate or deflate a kicker's overall percentage.
    /// </summary>
    private static double CalculateWeightedFgPct(SeasonFieldGoalStats s)
    {
        double weightedMade = s.Made19  * BracketWeights["1-19"]
                            + s.Made29  * BracketWeights["20-29"]
                            + s.Made39  * BracketWeights["30-39"]
                            + s.Made49  * BracketWeights["40-49"]
                            + s.Made50  * BracketWeights["50+"];

        double weightedAttempts = s.Attempts19 * BracketWeights["1-19"]
                                + s.Attempts29 * BracketWeights["20-29"]
                                + s.Attempts39 * BracketWeights["30-39"]
                                + s.Attempts49 * BracketWeights["40-49"]
                                + s.Attempts50 * BracketWeights["50+"];

        return Calc.Pct(weightedMade, weightedAttempts);
    }
}

/// <summary>
/// Calculates all derived kickoff metrics from a <see cref="SeasonKickoffStats"/> block.
/// </summary>
public static class KickoffCalculator
{
    /// <summary>
    /// Calculates all derived kickoff metrics. Returns <c>null</c> if <paramref name="stats"/> is <c>null</c>.
    /// </summary>
    public static KickoffStatsResult? Calculate(SeasonKickoffStats? stats)
    {
        if (stats is null) return null;

        int kickoffs = stats.Kickoffs;

        return new KickoffStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Kickoffs        = stats.Kickoffs,
            Endzone         = stats.Endzone,
            Inside20        = stats.Inside20,
            ReturnYards     = stats.ReturnYards,
            Returned        = stats.Returned,
            Touchbacks      = stats.Touchbacks,
            Yards           = stats.Yards,
            OutOfBounds     = stats.OutOfBounds,
            OnsideAttempts  = stats.OnsideAttempts,
            OnsideSuccesses = stats.OnsideSuccesses,

            TouchbackRate              = Calc.Round(Calc.Pct(stats.Touchbacks, kickoffs)),
            AverageDistance            = Calc.Round(Calc.Ratio(stats.Yards, kickoffs)),
            EndzoneRate                = Calc.Round(Calc.Pct(stats.Endzone, kickoffs)),
            Inside20Rate               = Calc.Round(Calc.Pct(stats.Inside20, kickoffs)),
            ShortFieldRate             = Calc.Round(Calc.Pct(stats.Touchbacks + stats.Inside20, kickoffs)),
            AverageReturnYardsAllowed  = Calc.Round(Calc.Ratio(stats.ReturnYards, stats.Returned)),
            OutOfBoundsRate            = Calc.Round(Calc.Pct(stats.OutOfBounds, kickoffs)),
            OnsideSuccessRate          = Calc.Round(Calc.Pct(stats.OnsideSuccesses, stats.OnsideAttempts)),
            SquibKicks                 = stats.SquibKicks,
        };
    }
}

/// <summary>
/// Calculates all derived punting metrics from a <see cref="SeasonPuntStats"/> block.
/// </summary>
public static class PuntingCalculator
{
    /// <summary>
    /// Calculates all derived punting metrics. Returns <c>null</c> if <paramref name="stats"/> is <c>null</c>.
    /// </summary>
    public static PuntingStatsResult? Calculate(SeasonPuntStats? stats)
    {
        if (stats is null) return null;

        int att     = stats.Attempts;
        int netYds  = stats.NetYards;
        int grossYds = stats.Yards;

        return new PuntingStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Attempts        = stats.Attempts,
            Yards           = stats.Yards,
            NetYards        = stats.NetYards,
            Blocked         = stats.Blocked,
            Touchbacks      = stats.Touchbacks,
            Inside20        = stats.Inside20,
            ReturnYards     = stats.ReturnYards,
            AvgNetYards     = stats.AvgNetYards,
            AvgYards        = stats.AvgYards,
            Longest         = stats.Longest,
            HangTime        = stats.HangTime,
            AvgHangTime     = stats.AvgHangTime,

            GrossAverage                = Calc.Round(Calc.Ratio(grossYds, att)),
            NetAverage                  = Calc.Round(Calc.Ratio(netYds, att)),
            GrossNetDelta               = Calc.Round(stats.AvgYards - stats.AvgNetYards),
            DistanceEfficiencyIndex     = Calc.Round(Calc.Pct(netYds, grossYds)),
            Inside20Rate                = Calc.Round(Calc.Pct(stats.Inside20, att)),
            TouchbackRate               = Calc.Round(Calc.Pct(stats.Touchbacks, att)),
            ShortFieldRate              = Calc.Round(Calc.Pct(stats.Inside20 + stats.Touchbacks, att)),
            BlockedRate                 = Calc.Round(Calc.Pct(stats.Blocked, att)),
            ReturnYardsAllowedPerPunt   = Calc.Round(Calc.Ratio(stats.ReturnYards, att)),
            AverageHangTime             = stats.AvgHangTime > 0
                                              ? Calc.Round(stats.AvgHangTime, 2)
                                              : Calc.Round(Calc.Ratio(stats.HangTime, att), 2),
        };
    }
}

/// <summary>
/// Calculates derived kick-return metrics from a <see cref="SeasonKickReturnStats"/> block.
/// </summary>
public static class KickReturnCalculator
{
    public static KickReturnStatsResult? Calculate(SeasonKickReturnStats? stats)
    {
        if (stats is null) return null;

        return new KickReturnStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            AvgYards        = stats.AvgYards,
            Yards           = stats.Yards,
            Longest         = stats.Longest,
            Touchdowns      = stats.Touchdowns,
            LongestTouchdown = stats.LongestTouchdown,
            Faircatches     = stats.Faircatches,
            Returns         = stats.Returns,

            AverageReturn = Calc.Round(Calc.Ratio(stats.Yards, stats.Returns)),
            TdRate        = Calc.Round(Calc.Pct(stats.Touchdowns, stats.Returns)),
            FairCatchRate = Calc.Round(Calc.Pct(stats.Faircatches, stats.Returns + stats.Faircatches)),
        };
    }
}

/// <summary>
/// Calculates derived punt-return metrics from a <see cref="SeasonPuntReturnStats"/> block.
/// </summary>
public static class PuntReturnCalculator
{
    public static PuntReturnStatsResult? Calculate(SeasonPuntReturnStats? stats)
    {
        if (stats is null) return null;

        return new PuntReturnStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            AvgYards        = stats.AvgYards,
            Returns         = stats.Returns,
            Yards           = stats.Yards,
            Longest         = stats.Longest,
            Touchdowns      = stats.Touchdowns,
            LongestTouchdown = stats.LongestTouchdown,
            Faircatches     = stats.Faircatches,

            AverageReturn = Calc.Round(Calc.Ratio(stats.Yards, stats.Returns)),
            TdRate        = Calc.Round(Calc.Pct(stats.Touchdowns, stats.Returns)),
            FairCatchRate = Calc.Round(Calc.Pct(stats.Faircatches, stats.Returns + stats.Faircatches)),
        };
    }
}

/// <summary>
/// Calculates derived interception-return metrics from a <see cref="SeasonIntReturnStats"/> block.
/// </summary>
public static class IntReturnCalculator
{
    public static IntReturnStatsResult? Calculate(SeasonIntReturnStats? stats)
    {
        if (stats is null) return null;

        return new IntReturnStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            AvgYards        = stats.AvgYards,
            Yards           = stats.Yards,
            Longest         = stats.Longest,
            Touchdowns      = stats.Touchdowns,
            LongestTouchdown = stats.LongestTouchdown,
            Returns         = stats.Returns,

            AverageReturn = Calc.Round(Calc.Ratio(stats.Yards, stats.Returns)),
            PickSixRate   = Calc.Round(Calc.Pct(stats.Touchdowns, stats.Returns)),
        };
    }
}

/// <summary>
/// Calculates derived miscellaneous-return metrics from a <see cref="SeasonMiscReturnStats"/> block.
/// </summary>
public static class MiscReturnCalculator
{
    public static MiscReturnStatsResult? Calculate(SeasonMiscReturnStats? stats)
    {
        if (stats is null) return null;

        return new MiscReturnStatsResult
        {
            // ── Raw Fields ────────────────────────────────────────────────
            Yards           = stats.Yards,
            Touchdowns      = stats.Touchdowns,
            LongestTouchdown = stats.LongestTouchdown,
            Returns         = stats.Returns,

            AverageReturnYards     = Calc.Round(Calc.Ratio(stats.Yards, stats.Returns)),
            TotalMiscTouchdowns    = stats.BlkFgTouchdowns + stats.BlkPuntTouchdowns
                                        + stats.FgReturnTouchdowns + stats.EzRecTouchdowns,
            BlockedFgTouchdowns    = stats.BlkFgTouchdowns,
            BlockedPuntTouchdowns  = stats.BlkPuntTouchdowns,
            FgReturnTouchdowns     = stats.FgReturnTouchdowns,
            EzRecTouchdowns        = stats.EzRecTouchdowns,
        };
    }
}
