using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Domain.Transformers;

public class SeasonalStatsTransformer(SeasonalStatsApiResponse dto)
{
    public (TeamSeasonStats Team, IReadOnlyList<Player> Players, IReadOnlyList<PlayerSeasonStats> PlayerStats) Transform()
    {
        var teamStats   = MapTeamStats();
        var players     = MapPlayers(teamStats.TeamId);
        var playerStats = MapPlayerStats(teamStats.TeamId, teamStats.SeasonYear, teamStats.SeasonType, teamStats.SeasonSrId);
        return (teamStats, players, playerStats);
    }

    // ── Team ─────────────────────────────────────────────────────────────

    private TeamSeasonStats MapTeamStats() => new()
    {
        Id         = Guid.NewGuid(),
        TeamId     = dto.Id,
        SeasonSrId = dto.Season.Id?.ToString(),
        SeasonYear = dto.Season.Year,
        SeasonType = dto.Season.Type,
        GamesPlayed = dto.Record?.GamesPlayed ?? dto.Opponents?.GamesPlayed ?? 0,
        Record    = MapTeamStatBlock(dto.Record),
        Opponents = MapTeamStatBlock(dto.Opponents),
    };

    private static TeamStatBlock MapTeamStatBlock(SeasonStatsRecordDto? record) => new()
    {
        Touchdowns   = MapTouchdowns(record?.Touchdowns),
        Rushing      = MapRushing(record?.Rushing),
        Passing      = MapPassing(record?.Passing),
        Receiving    = MapReceiving(record?.Receiving),
        Defense      = MapDefense(record?.Defense),
        FieldGoals   = MapFieldGoals(record?.FieldGoals),
        Kickoffs     = MapKickoffs(record?.Kickoffs),
        KickReturns  = MapKickReturns(record?.KickReturns),
        Punts        = MapPunts(record?.Punts),
        PuntReturns  = MapPuntReturns(record?.PuntReturns),
        Interceptions = MapInterceptions(record?.Interceptions),
        IntReturns   = MapIntReturns(record?.IntReturns),
        Fumbles      = MapFumbles(record?.Fumbles),
        FirstDowns   = MapFirstDowns(record?.FirstDowns),
        Penalties    = MapPenalties(record?.Penalties),
        MiscReturns  = MapMiscReturns(record?.MiscReturns),
        ExtraPoints  = MapTeamExtraPoints(record?.ExtraPoints),
        Efficiency   = MapEfficiency(record?.Efficiency),
    };

    // ── Players ───────────────────────────────────────────────────────────

    private IReadOnlyList<Player> MapPlayers(Guid teamId) =>
        dto.Players.Select(p =>
        {
            var (firstName, lastName) = SplitName(p.Name);
            return new Player
            {
                Id        = p.Id,
                TeamId    = teamId,
                Name      = p.Name,
                FirstName = firstName,
                LastName  = lastName,
                Jersey    = p.Jersey,
                Position  = Enum.TryParse<PlayerPosition>(p.Position, out var pos) ? pos : null,
                SrId      = p.SrId,
            };
        }).ToList();

    private static (string first, string last) SplitName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return ("", "");
        var idx = name.LastIndexOf(' ');
        return idx < 0
            ? (name, "")
            : (name[..idx], name[(idx + 1)..]);
    }

    private IReadOnlyList<PlayerSeasonStats> MapPlayerStats(
        Guid teamId, int seasonYear, string seasonType, string? seasonSrId)
    {
        return dto.Players.Select(p => new PlayerSeasonStats
        {
            Id          = Guid.NewGuid(),
            PlayerId    = p.Id,
            TeamId      = teamId,
            PlayerSrId  = p.SrId,
            SeasonSrId  = seasonSrId,
            SeasonYear  = seasonYear,
            SeasonType  = seasonType,
            GamesPlayed = p.GamesPlayed,
            GamesStarted = p.GamesStarted,
            Stats       = MapPlayerStatBlock(p),
        }).ToList();
    }

    private static PlayerStatBlock MapPlayerStatBlock(SeasonPlayerDto p) => new()
    {
        Rushing     = MapRushing(p.Rushing),
        Passing     = MapPassing(p.Passing),
        Receiving   = MapReceiving(p.Receiving),
        Defense     = MapDefense(p.Defense),
        FieldGoals  = MapFieldGoals(p.FieldGoals),
        ExtraPoints = MapPlayerExtraPoints(p.ExtraPoints),
        Punts       = MapPunts(p.Punts),
        Kickoffs    = MapKickoffs(p.Kickoffs),
        PuntReturns = MapPuntReturns(p.PuntReturns),
        KickReturns = MapKickReturns(p.KickReturns),
        MiscReturns = MapMiscReturns(p.MiscReturns),
        Fumbles     = MapFumbles(p.Fumbles),
        Penalties   = MapPenalties(p.Penalties),
        IntReturns  = MapIntReturns(p.IntReturns),
    };

    // ── Stat-block mappers ────────────────────────────────────────────────

    private static SeasonTouchdownStats? MapTouchdowns(SeasonTouchdownsDto? d) =>
        d is null ? null : new()
        {
            Pass         = d.Pass,
            Rush         = d.Rush,
            TotalReturn  = d.TotalReturn,
            Total        = d.Total,
            FumbleReturn = d.FumbleReturn,
            IntReturn    = d.IntReturn,
            KickReturn   = d.KickReturn,
            PuntReturn   = d.PuntReturn,
            Other        = d.Other,
        };

    private static SeasonRushingStats? MapRushing(SeasonRushingDto? d) =>
        d is null ? null : new()
        {
            AvgYards         = d.AvgYards,
            Attempts         = d.Attempts,
            Touchdowns       = d.Touchdowns,
            Tlost            = d.Tlost,
            TlostYards       = d.TlostYards,
            Yards            = d.Yards,
            Longest          = d.Longest,
            LongestTouchdown = d.LongestTouchdown,
            RedzoneAttempts  = d.RedzoneAttempts,
            BrokenTackles    = d.BrokenTackles,
            KneelDowns       = d.KneelDowns,
            Scrambles        = d.Scrambles,
            YardsAfterContact = d.YardsAfterContact,
            FirstDowns       = d.FirstDowns,
        };

    private static SeasonPassingStats? MapPassing(SeasonPassingDto? d) =>
        d is null ? null : new()
        {
            Attempts         = d.Attempts,
            Completions      = d.Completions,
            CmpPct           = d.CmpPct,
            Interceptions    = d.Interceptions,
            SackYards        = d.SackYards,
            Rating           = d.Rating,
            Touchdowns       = d.Touchdowns,
            AvgYards         = d.AvgYards,
            Sacks            = d.Sacks,
            Longest          = d.Longest,
            LongestTouchdown = d.LongestTouchdown,
            AirYards         = d.AirYards,
            RedzoneAttempts  = d.RedzoneAttempts,
            NetYards         = d.NetYards,
            Yards            = d.Yards,
            GrossYards       = d.GrossYards,
            IntTouchdowns    = d.IntTouchdowns,
            ThrowAways       = d.ThrowAways,
            PoorThrows       = d.PoorThrows,
            DefendedPasses   = d.DefendedPasses,
            DroppedPasses    = d.DroppedPasses,
            Spikes           = d.Spikes,
            Blitzes          = d.Blitzes,
            Hurries          = d.Hurries,
            Knockdowns       = d.Knockdowns,
            PocketTime       = d.PocketTime,
            BattedPasses     = d.BattedPasses,
            OnTargetThrows   = d.OnTargetThrows,
            FirstDowns       = d.FirstDowns,
            AvgPocketTime    = d.AvgPocketTime,
        };

    private static SeasonReceivingStats? MapReceiving(SeasonReceivingDto? d) =>
        d is null ? null : new()
        {
            Targets          = d.Targets,
            Receptions       = d.Receptions,
            AvgYards         = d.AvgYards,
            Yards            = d.Yards,
            Touchdowns       = d.Touchdowns,
            YardsAfterCatch  = d.YardsAfterCatch,
            Longest          = d.Longest,
            LongestTouchdown = d.LongestTouchdown,
            RedzoneTargets   = d.RedzoneTargets,
            AirYards         = d.AirYards,
            BrokenTackles    = d.BrokenTackles,
            DroppedPasses    = d.DroppedPasses,
            CatchablePasses  = d.CatchablePasses,
            YardsAfterContact = d.YardsAfterContact,
            FirstDowns       = d.FirstDowns,
        };

    private static SeasonDefenseStats? MapDefense(SeasonDefenseDto? d) =>
        d is null ? null : new()
        {
            Tackles               = d.Tackles,
            Assists               = d.Assists,
            Combined              = d.Combined,
            Sacks                 = d.Sacks,
            SackYards             = d.SackYards,
            Interceptions         = d.Interceptions,
            PassesDefended        = d.PassesDefended,
            ForcedFumbles         = d.ForcedFumbles,
            FumbleRecoveries      = d.FumbleRecoveries,
            QbHits                = d.QbHits,
            Tloss                 = d.Tloss,
            TlossYards            = d.TlossYards,
            Safeties              = d.Safeties,
            SpTackles             = d.SpTackles,
            SpAssists             = d.SpAssists,
            SpForcedFumbles       = d.SpForcedFumbles,
            SpFumbleRecoveries    = d.SpFumbleRecoveries,
            SpBlocks              = d.SpBlocks,
            MiscTackles           = d.MiscTackles,
            MiscAssists           = d.MiscAssists,
            MiscForcedFumbles     = d.MiscForcedFumbles,
            MiscFumbleRecoveries  = d.MiscFumbleRecoveries,
            SpOwnFumbleRecoveries = d.SpOwnFumbleRecoveries,
            SpOppFumbleRecoveries = d.SpOppFumbleRecoveries,
            ThreeAndOutsForced    = d.ThreeAndOutsForced,
            FourthDownStops       = d.FourthDownStops,
            DefTargets            = d.DefTargets,
            DefComps              = d.DefComps,
            Blitzes               = d.Blitzes,
            Hurries               = d.Hurries,
            Knockdowns            = d.Knockdowns,
            MissedTackles         = d.MissedTackles,
            BattedPasses          = d.BattedPasses,
        };

    private static SeasonFieldGoalStats? MapFieldGoals(SeasonFieldGoalsDto? d) =>
        d is null ? null : new()
        {
            Attempts   = d.Attempts,
            Made       = d.Made,
            Blocked    = d.Blocked,
            Yards      = d.Yards,
            AvgYards   = d.AvgYards,
            Longest    = d.Longest,
            Missed     = d.Missed,
            Pct        = d.Pct,
            Attempts19 = d.Attempts19,
            Attempts29 = d.Attempts29,
            Attempts39 = d.Attempts39,
            Attempts49 = d.Attempts49,
            Attempts50 = d.Attempts50,
            Made19     = d.Made19,
            Made29     = d.Made29,
            Made39     = d.Made39,
            Made49     = d.Made49,
            Made50     = d.Made50,
        };

    private static SeasonKickoffStats? MapKickoffs(SeasonKickoffsDto? d) =>
        d is null ? null : new()
        {
            Kickoffs        = d.Kickoffs,
            Endzone         = d.Endzone,
            Inside20        = d.Inside20,
            ReturnYards     = d.ReturnYards,
            Returned        = d.Returned,
            Touchbacks      = d.Touchbacks,
            Yards           = d.Yards,
            OutOfBounds     = d.OutOfBounds,
            OnsideAttempts  = d.OnsideAttempts,
            OnsideSuccesses = d.OnsideSuccesses,
            SquibKicks      = d.SquibKicks,
        };

    private static SeasonKickReturnStats? MapKickReturns(SeasonKickReturnsDto? d) =>
        d is null ? null : new()
        {
            AvgYards         = d.AvgYards,
            Yards            = d.Yards,
            Longest          = d.Longest,
            Touchdowns       = d.Touchdowns,
            LongestTouchdown = d.LongestTouchdown,
            Faircatches      = d.Faircatches,
            Returns          = d.Returns,
        };

    private static SeasonPuntStats? MapPunts(SeasonPuntsDto? d) =>
        d is null ? null : new()
        {
            Attempts    = d.Attempts,
            Yards       = d.Yards,
            NetYards    = d.NetYards,
            Blocked     = d.Blocked,
            Touchbacks  = d.Touchbacks,
            Inside20    = d.Inside20,
            ReturnYards = d.ReturnYards,
            AvgNetYards = d.AvgNetYards,
            AvgYards    = d.AvgYards,
            Longest     = d.Longest,
            HangTime    = d.HangTime,
            AvgHangTime = d.AvgHangTime,
        };

    private static SeasonPuntReturnStats? MapPuntReturns(SeasonPuntReturnsDto? d) =>
        d is null ? null : new()
        {
            AvgYards         = d.AvgYards,
            Returns          = d.Returns,
            Yards            = d.Yards,
            Longest          = d.Longest,
            Touchdowns       = d.Touchdowns,
            LongestTouchdown = d.LongestTouchdown,
            Faircatches      = d.Faircatches,
        };

    private static SeasonInterceptionStats? MapInterceptions(SeasonInterceptionsDto? d) =>
        d is null ? null : new()
        {
            ReturnYards   = d.ReturnYards,
            Returned      = d.Returned,
            Interceptions = d.Interceptions,
        };

    private static SeasonIntReturnStats? MapIntReturns(SeasonIntReturnsDto? d) =>
        d is null ? null : new()
        {
            AvgYards         = d.AvgYards,
            Yards            = d.Yards,
            Longest          = d.Longest,
            Touchdowns       = d.Touchdowns,
            LongestTouchdown = d.LongestTouchdown,
            Returns          = d.Returns,
        };

    private static SeasonFumbleStats? MapFumbles(SeasonFumblesDto? d) =>
        d is null ? null : new()
        {
            Fumbles       = d.Fumbles,
            LostFumbles   = d.LostFumbles,
            OwnRec        = d.OwnRec,
            OwnRecYards   = d.OwnRecYards,
            OppRec        = d.OppRec,
            OppRecYards   = d.OppRecYards,
            OutOfBounds   = d.OutOfBounds,
            ForcedFumbles = d.ForcedFumbles,
            OwnRecTds     = d.OwnRecTds,
            OppRecTds     = d.OppRecTds,
            EzRecTds      = d.EzRecTds,
        };

    private static SeasonFirstDownStats? MapFirstDowns(SeasonFirstDownsDto? d) =>
        d is null ? null : new()
        {
            Pass    = d.Pass,
            Penalty = d.Penalty,
            Rush    = d.Rush,
            Total   = d.Total,
        };

    private static SeasonPenaltyStats? MapPenalties(SeasonPenaltiesDto? d) =>
        d is null ? null : new()
        {
            Penalties  = d.Penalties,
            Yards      = d.Yards,
            FirstDowns = d.FirstDowns,
        };

    private static SeasonMiscReturnStats? MapMiscReturns(SeasonMiscReturnsDto? d) =>
        d is null ? null : new()
        {
            Yards              = d.Yards,
            Touchdowns         = d.Touchdowns,
            LongestTouchdown   = d.LongestTouchdown,
            BlkFgTouchdowns    = d.BlkFgTouchdowns,
            BlkPuntTouchdowns  = d.BlkPuntTouchdowns,
            FgReturnTouchdowns = d.FgReturnTouchdowns,
            EzRecTouchdowns    = d.EzRecTouchdowns,
            Returns            = d.Returns,
        };

    private static SeasonTeamExtraPointStats? MapTeamExtraPoints(SeasonTeamExtraPointsDto? d) =>
        d is null ? null : new()
        {
            Kicks = d.Kicks is null ? null : new SeasonEpKickStats
            {
                Attempts = d.Kicks.Attempts,
                Blocked  = d.Kicks.Blocked,
                Made     = d.Kicks.Made,
                Pct      = d.Kicks.Pct,
            },
            Conversions = d.Conversions is null ? null : new SeasonEpConversionStats
            {
                PassAttempts     = d.Conversions.PassAttempts,
                PassSuccesses    = d.Conversions.PassSuccesses,
                RushAttempts     = d.Conversions.RushAttempts,
                RushSuccesses    = d.Conversions.RushSuccesses,
                DefenseAttempts  = d.Conversions.DefenseAttempts,
                DefenseSuccesses = d.Conversions.DefenseSuccesses,
                TurnoverSuccesses = d.Conversions.TurnoverSuccesses,
            },
        };

    private static SeasonPlayerExtraPointStats? MapPlayerExtraPoints(SeasonPlayerExtraPointsDto? d) =>
        d is null ? null : new()
        {
            Attempts = d.Attempts,
            Made     = d.Made,
            Blocked  = d.Blocked,
            Missed   = d.Missed,
            Pct      = d.Pct,
        };

    private static SeasonEfficiencyStats? MapEfficiency(SeasonEfficiencyDto? d) =>
        d is null ? null : new()
        {
            Goaltogo   = MapEfficiencyBlock(d.Goaltogo),
            Redzone    = MapEfficiencyBlock(d.Redzone),
            Thirddown  = MapEfficiencyBlock(d.Thirddown),
            Fourthdown = MapEfficiencyBlock(d.Fourthdown),
        };

    private static SeasonEfficiencyBlock? MapEfficiencyBlock(SeasonEfficiencyBlockDto? d) =>
        d is null ? null : new() { Attempts = d.Attempts, Successes = d.Successes, Pct = d.Pct };
}
