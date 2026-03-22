using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Fluent builders that produce realistic <see cref="PlayerStatBlock"/> and
/// <see cref="TeamStatBlock"/> fixtures for unit tests.
/// All builder methods return <c>this</c> so calls can be chained.
/// </summary>
internal static class StatBlockBuilder
{
    // ─────────────────────────────────────────────────────────────────────────
    // Passing
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// A prototypical elite-QB passing season: 65% completion, ~112 passer rating,
    /// strong ANY/A, minimal negative plays.
    /// </summary>
    internal static SeasonPassingStats EliteQbPassing() => new()
    {
        Attempts          = 600,
        Completions       = 390,     // 65.0 %
        Yards             = 4800,
        GrossYards        = 4800,
        NetYards          = 4610,    // 190 sack yards lost
        Touchdowns        = 38,
        Interceptions     = 10,
        Sacks             = 30,
        SackYards         = 190,
        AirYards          = 3200,
        RedzoneAttempts   = 90,
        ThrowAways        = 24,
        PoorThrows        = 42,
        DroppedPasses     = 18,
        Spikes            = 6,
        Blitzes           = 150,
        Hurries           = 36,
        Knockdowns        = 12,
        BattedPasses      = 8,
        DefendedPasses    = 65,
        OnTargetThrows    = 510,
        PocketTime        = 1_560,   // total seconds → 2.60s avg over 600
        AvgPocketTime     = 2.60,
        FirstDowns        = 220,
        IntTouchdowns     = 2,
        Rating            = 112.4m,
        CmpPct            = 65.0m,
        AvgYards          = 8.0m,
    };

    /// <summary>
    /// A backup/struggling QB with poor ball security.
    /// </summary>
    internal static SeasonPassingStats PoorQbPassing() => new()
    {
        Attempts       = 200,
        Completions    = 110,
        Yards          = 1200,
        GrossYards     = 1200,
        NetYards       = 1080,
        Touchdowns     = 5,
        Interceptions  = 12,
        Sacks          = 22,
        SackYards      = 120,
        AirYards       = 780,
        ThrowAways     = 5,
        PoorThrows     = 38,
        DroppedPasses  = 6,
        Blitzes        = 60,
        Hurries        = 28,
        OnTargetThrows = 120,
        Rating         = 60.2m,
        CmpPct         = 55.0m,
        AvgYards       = 6.0m,
    };

    // ─────────────────────────────────────────────────────────────────────────
    // Rushing
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>A workhorse RB season: high volume, strong YAC, redzone threat.</summary>
    internal static SeasonRushingStats WorkhorseRbRushing() => new()
    {
        Attempts           = 280,
        Yards              = 1350,
        Touchdowns         = 14,
        YardsAfterContact  = 700,
        BrokenTackles      = 42,
        TacklesForLoss     = 14m,
        TacklesForLossYards = 42,
        RedzoneAttempts    = 55,
        KneelDowns         = 0,
        Scrambles          = 0,
        Longest            = 62,
        FirstDowns         = 80,
    };

    /// <summary>A QB who scrambles but also takes kneel-downs.</summary>
    internal static SeasonRushingStats ScrambleQbRushing() => new()
    {
        Attempts           = 80,
        Yards              = 480,
        Touchdowns         = 5,
        YardsAfterContact  = 200,
        BrokenTackles      = 10,
        TacklesForLoss     = 3m,
        TacklesForLossYards = 8,
        RedzoneAttempts    = 12,
        KneelDowns         = 16,
        Scrambles          = 32,
        Longest            = 41,
        FirstDowns         = 28,
    };

    // ─────────────────────────────────────────────────────────────────────────
    // Receiving
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>A WR1 season: high targets, elite YAC, low drop rate.</summary>
    internal static SeasonReceivingStats WrOneReceiving() => new()
    {
        Targets           = 155,
        Receptions        = 105,
        Yards             = 1350,
        Touchdowns        = 10,
        YardsAfterCatch   = 480,
        YardsAfterContact = 210,
        AirYards          = 1800,
        BrokenTackles     = 22,
        DroppedPasses     = 6,
        CatchablePasses   = 140,
        RedzoneTargets    = 22,
        Longest           = 75,
        FirstDowns        = 68,
    };

    /// <summary>A pass-catching RB: high catch rate, low aDOT, big YAC.</summary>
    internal static SeasonReceivingStats PassCatchingRbReceiving() => new()
    {
        Targets           = 80,
        Receptions        = 68,
        Yards             = 550,
        Touchdowns        = 4,
        YardsAfterCatch   = 390,
        YardsAfterContact = 280,
        AirYards          = 240,   // very low aDOT — dump-offs
        BrokenTackles     = 18,
        DroppedPasses     = 3,
        CatchablePasses   = 76,
        RedzoneTargets    = 8,
        FirstDowns        = 32,
    };

    // ─────────────────────────────────────────────────────────────────────────
    // Defense
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>An edge rusher with elite pass rush production.</summary>
    internal static SeasonDefenseStats EdgeRusherDefense() => new()
    {
        Tackles            = 42,
        Assists            = 16,
        Combined           = 58,
        Sacks              = 14.5m,
        SackYards          = 98m,
        QbHits             = 28,
        Hurries            = 36,
        Knockdowns         = 8,
        BattedPasses       = 5,
        ForcedFumbles      = 4,
        FumbleRecoveries   = 2,
        Tloss              = 18m,
        TlossYards         = 62m,
        PassesDefended     = 3,
        Interceptions      = 0,
        Blitzes            = 110,
        MissedTackles      = 4,
        DefTargets         = 0,    // edge rushers rarely tracked in coverage
        DefComps           = 0,
        SpTackles          = 0,
        SpAssists          = 0,
        SpBlocks           = 0,
        SpForcedFumbles    = 0,
        FourthDownStops    = 3,
        ThreeAndOutsForced = 12,
        Safeties           = 1,
    };

    /// <summary>A shutdown corner with excellent coverage metrics.</summary>
    internal static SeasonDefenseStats ShutdownCbDefense() => new()
    {
        Tackles          = 38,
        Assists          = 10,
        Combined         = 48,
        Sacks            = 0m,
        SackYards        = 0m,
        QbHits           = 1,
        Hurries          = 2,
        ForcedFumbles    = 1,
        FumbleRecoveries = 1,
        PassesDefended   = 18,
        Interceptions    = 5,
        Blitzes          = 12,
        Tloss            = 2m,
        TlossYards       = 4m,
        DefTargets       = 68,
        DefComps         = 28,   // 41% comp% allowed
        MissedTackles    = 3,
        SpTackles        = 8,
        SpAssists        = 4,
        SpBlocks         = 0,
        SpForcedFumbles  = 1,
        FourthDownStops  = 1,
        ThreeAndOutsForced = 4,
        Safeties         = 0,
    };

    // ─────────────────────────────────────────────────────────────────────────
    // Kicker / Special Teams
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>An elite kicker: high volume, excellent distance bracket accuracy.</summary>
    internal static SeasonFieldGoalStats EliteKickerFg() => new()
    {
        Attempts   = 36,
        Made       = 33,
        Missed     = 2,
        Blocked    = 1,
        Yards      = 1512,   // avg ~42 yds
        Longest    = 58,
        Attempts19 = 2,  Made19 = 2,
        Attempts29 = 6,  Made29 = 6,
        Attempts39 = 8,  Made39 = 8,
        Attempts49 = 12, Made49 = 11,
        Attempts50 = 8,  Made50 = 6,
    };

    internal static SeasonPlayerExtraPointStats ReliableXp() => new()
    {
        Attempts = 48,
        Made     = 46,
        Missed   = 1,
        Blocked  = 1,
    };

    internal static SeasonKickoffStats StrongLegKickoffs() => new()
    {
        Kickoffs       = 72,
        Yards          = 4680,   // avg 65 yds
        Touchbacks     = 48,
        Endzone        = 60,
        Inside20       = 8,
        Returned       = 16,
        ReturnYards    = 352,
        OutOfBounds    = 2,
        OnsideAttempts = 2,
        OnsideSuccesses = 1,
        SquibKicks     = 3,
    };

    internal static SeasonPuntStats ElitePunter() => new()
    {
        Attempts    = 62,
        Yards       = 2914,     // gross avg 47.0
        NetYards    = 2604,     // net avg 42.0
        Blocked     = 0,
        Touchbacks  = 4,
        Inside20    = 26,
        ReturnYards = 248,
        AvgYards    = 47.0m,
        AvgNetYards = 42.0m,
        HangTime    = 272.8,    // total seconds
        AvgHangTime = 4.4,
        Longest     = 68,
    };

    internal static SeasonKickReturnStats GoodKickReturner() => new()
    {
        Returns    = 28,
        Yards      = 728,    // 26.0 avg
        Touchdowns = 1,
        Faircatches = 4,
        Longest    = 98,
    };

    internal static SeasonPuntReturnStats GoodPuntReturner() => new()
    {
        Returns    = 30,
        Yards      = 330,    // 11.0 avg
        Touchdowns = 1,
        Faircatches = 12,
        Longest    = 78,
    };

    internal static SeasonIntReturnStats IntReturnStats() => new()
    {
        Returns    = 5,
        Yards      = 88,
        Touchdowns = 2,
        Longest    = 44,
    };

    internal static SeasonFumbleStats FumbleProneFumbles() => new()
    {
        Fumbles       = 6,
        LostFumbles   = 4,
        OwnRec        = 2,
        OppRec        = 4,
        ForcedFumbles = 0,
        OwnRecTds     = 0,
        OppRecTds     = 0,
        EzRecTds      = 0,
    };

    internal static SeasonFumbleStats SecureFumbles() => new()
    {
        Fumbles       = 1,
        LostFumbles   = 0,
        OwnRec        = 1,
        ForcedFumbles = 2,
    };

    internal static SeasonPenaltyStats PenaltyStats(int count = 8, int yards = 60, int? firstDowns = null)
        => new() { Penalties = count, Yards = yards, FirstDowns = firstDowns };

    // ─────────────────────────────────────────────────────────────────────────
    // Team-only blocks
    // ─────────────────────────────────────────────────────────────────────────

    internal static SeasonTouchdownStats TeamTouchdowns() => new()
    {
        Pass         = 32,
        Rush         = 14,
        TotalReturn  = 4,
        KickReturn   = 1,
        PuntReturn   = 2,
        FumbleReturn = 1,
        IntReturn    = 1,
        Other        = 0,
        Total        = 50,
    };

    internal static SeasonFirstDownStats TeamFirstDowns() => new()
    {
        Pass    = 180,
        Rush    = 90,
        Penalty = 30,
        Total   = 300,
    };

    internal static SeasonEfficiencyStats TeamEfficiency() => new()
    {
        Thirddown  = new SeasonEfficiencyBlock { Attempts = 200, Successes = 88,  Pct = 44.0m },
        Fourthdown = new SeasonEfficiencyBlock { Attempts = 18,  Successes = 10,  Pct = 55.6m },
        Redzone    = new SeasonEfficiencyBlock { Attempts = 55,  Successes = 44,  Pct = 80.0m },
        Goaltogo   = new SeasonEfficiencyBlock { Attempts = 28,  Successes = 20,  Pct = 71.4m },
    };

    internal static SeasonTeamExtraPointStats TeamExtraPoints() => new()
    {
        Kicks = new SeasonEpKickStats
        {
            Attempts = 46, Made = 44, Blocked = 1, Pct = 95.7m,
        },
        Conversions = new SeasonEpConversionStats
        {
            PassAttempts     = 6, PassSuccesses    = 3,
            RushAttempts     = 4, RushSuccesses    = 2,
            DefenseAttempts  = 1, DefenseSuccesses = 1,
            TurnoverSuccesses = 0,
        },
    };

    internal static SeasonInterceptionStats TeamInterceptions() => new()
    {
        Interceptions = 18,
        Returned      = 14,
        ReturnYards   = 210,
    };

    internal static SeasonMiscReturnStats MiscReturns() => new()
    {
        Returns              = 3,
        Yards                = 72,
        Touchdowns           = 2,
        BlkFgTouchdowns      = 0,
        BlkPuntTouchdowns    = 1,
        FgReturnTouchdowns   = 0,
        EzRecTouchdowns      = 1,
        LongestTouchdown     = 61,
    };
}
