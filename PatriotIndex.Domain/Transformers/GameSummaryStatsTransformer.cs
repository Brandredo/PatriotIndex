using System.Text.Json;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Transformers;

public class GameSummaryStatsTransformer(string json)
{
    public (List<TeamGameStats> TeamStats, List<PlayerGameStats> PlayerStats) Transform()
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var gameId = GetGuid(root, "id");
        if (!root.TryGetProperty("statistics", out var statistics))
            return ([], []);

        var teamStatsList   = new List<TeamGameStats>();
        var allPlayerStats  = new List<PlayerGameStats>();

        foreach (var (side, isHome) in new[] { ("home", true), ("away", false) })
        {
            if (!statistics.TryGetProperty(side, out var teamEl)) continue;

            var teamId = GetGuid(teamEl, "id");

            teamStatsList.Add(new TeamGameStats
            {
                Id     = Guid.NewGuid(),
                GameId = gameId,
                TeamId = teamId,
                IsHome = isHome,
                Stats  = BuildTeamBlock(teamEl),
            });

            var playerMap = new Dictionary<Guid, PlayerGameStatsBlock>();
            CollectPlayerStats(teamEl, playerMap);

            foreach (var (playerId, block) in playerMap)
            {
                allPlayerStats.Add(new PlayerGameStats
                {
                    Id       = Guid.NewGuid(),
                    GameId   = gameId,
                    PlayerId = playerId,
                    TeamId   = teamId,
                    Stats    = block,
                });
            }
        }

        return (teamStatsList, allPlayerStats);
    }

    // ── Team block ──────────────────────────────────────────────────────────

    private static TeamGameStatsBlock BuildTeamBlock(JsonElement team) => new()
    {
        Summary     = BuildSummary(team),
        Rushing     = MapRushing(Totals(team, "rushing")),
        Passing     = MapPassing(Totals(team, "passing")),
        Receiving   = MapReceiving(Totals(team, "receiving")),
        Defense     = MapDefense(Totals(team, "defense")),
        FieldGoals  = MapFieldGoals(Totals(team, "field_goals")),
        Punts       = MapPunts(Totals(team, "punts")),
        Kickoffs    = MapKickoffs(Totals(team, "kickoffs")),
        PuntReturns = MapPuntReturns(Totals(team, "punt_returns")),
        KickReturns = MapKickReturns(Totals(team, "kick_returns")),
        IntReturns  = MapIntReturns(Totals(team, "int_returns")),
        Fumbles     = MapFumbles(Totals(team, "fumbles")),
        Penalties   = MapPenalties(Totals(team, "penalties")),
    };

    private static TeamGameSummary? BuildSummary(JsonElement team)
    {
        if (!team.TryGetProperty("summary", out var s)) return null;
        return new TeamGameSummary
        {
            PossessionTime = GetStrN(s, "possession_time"),
            AvgGain        = GetDouble(s, "avg_gain"),
            Safeties       = GetInt(s, "safeties"),
            Turnovers      = GetInt(s, "turnovers"),
            PlayCount      = GetInt(s, "play_count"),
            RushPlays      = GetInt(s, "rush_plays"),
            TotalYards     = GetInt(s, "total_yards"),
            Fumbles        = GetInt(s, "fumbles"),
            Penalties      = GetInt(s, "penalties"),
            ReturnYards    = GetInt(s, "return_yards"),
        };
    }

    // ── Player collection ────────────────────────────────────────────────────

    private static void CollectPlayerStats(JsonElement team, Dictionary<Guid, PlayerGameStatsBlock> map)
    {
        ProcessCategory(team, "rushing",      map, (el, b) => b.Rushing     = MapRushing(el));
        ProcessCategory(team, "passing",      map, (el, b) => b.Passing     = MapPassing(el));
        ProcessCategory(team, "receiving",    map, (el, b) => b.Receiving   = MapReceiving(el));
        ProcessCategory(team, "defense",      map, (el, b) => b.Defense     = MapDefense(el));
        ProcessCategory(team, "field_goals",  map, (el, b) => b.FieldGoals  = MapFieldGoals(el));
        ProcessCategory(team, "punts",        map, (el, b) => b.Punts       = MapPunts(el));
        ProcessCategory(team, "kickoffs",     map, (el, b) => b.Kickoffs    = MapKickoffs(el));
        ProcessCategory(team, "punt_returns", map, (el, b) => b.PuntReturns = MapPuntReturns(el));
        ProcessCategory(team, "kick_returns", map, (el, b) => b.KickReturns = MapKickReturns(el));
        ProcessCategory(team, "int_returns",  map, (el, b) => b.IntReturns  = MapIntReturns(el));
        ProcessCategory(team, "fumbles",      map, (el, b) => b.Fumbles     = MapFumbles(el));
        ProcessCategory(team, "penalties",    map, (el, b) => b.Penalties   = MapPenalties(el));
    }

    private static void ProcessCategory(
        JsonElement team,
        string category,
        Dictionary<Guid, PlayerGameStatsBlock> map,
        Action<JsonElement, PlayerGameStatsBlock> setter)
    {
        if (!team.TryGetProperty(category, out var cat)) return;
        if (!cat.TryGetProperty("players", out var players)) return;
        if (players.ValueKind != JsonValueKind.Array) return;

        foreach (var p in players.EnumerateArray())
        {
            var id = GetGuid(p, "id");
            if (id == Guid.Empty) continue;
            if (!map.TryGetValue(id, out var block))
            {
                block    = new PlayerGameStatsBlock();
                map[id] = block;
            }
            setter(p, block);
        }
    }

    // ── Stat mappers (totals and player entries share the same field names) ─

    private static SeasonRushingStats? MapRushing(JsonElement? d) =>
        d is null ? null : new SeasonRushingStats
        {
            AvgYards           = GetDouble(d.Value, "avg_yards"),
            Attempts           = GetInt(d.Value, "attempts"),
            Touchdowns         = GetInt(d.Value, "touchdowns"),
            TacklesForLoss     = GetDecimal(d.Value, "tlost"),
            TacklesForLossYards = GetInt(d.Value, "tlost_yards"),
            Yards              = GetInt(d.Value, "yards"),
            Longest            = GetInt(d.Value, "longest"),
            LongestTouchdown   = GetInt(d.Value, "longest_touchdown"),
            RedzoneAttempts    = GetInt(d.Value, "redzone_attempts"),
            BrokenTackles      = GetInt(d.Value, "broken_tackles"),
            KneelDowns         = GetInt(d.Value, "kneel_downs"),
            Scrambles          = GetInt(d.Value, "scrambles"),
            YardsAfterContact  = GetInt(d.Value, "yards_after_contact"),
            FirstDowns         = GetIntN(d.Value, "first_downs"),
        };

    private static SeasonPassingStats? MapPassing(JsonElement? d) =>
        d is null ? null : new SeasonPassingStats
        {
            Attempts         = GetInt(d.Value, "attempts"),
            Completions      = GetInt(d.Value, "completions"),
            CmpPct           = GetDecimal(d.Value, "cmp_pct"),
            Interceptions    = GetInt(d.Value, "interceptions"),
            SackYards        = GetInt(d.Value, "sack_yards"),
            Rating           = GetDecimal(d.Value, "rating"),
            Touchdowns       = GetInt(d.Value, "touchdowns"),
            AvgYards         = GetDecimal(d.Value, "avg_yards"),
            Sacks            = GetInt(d.Value, "sacks"),
            Longest          = GetInt(d.Value, "longest"),
            LongestTouchdown = GetInt(d.Value, "longest_touchdown"),
            AirYards         = GetInt(d.Value, "air_yards"),
            RedzoneAttempts  = GetInt(d.Value, "redzone_attempts"),
            NetYards         = GetInt(d.Value, "net_yards"),
            Yards            = GetInt(d.Value, "yards"),
            IntTouchdowns    = GetInt(d.Value, "int_touchdowns"),
            ThrowAways       = GetInt(d.Value, "throw_aways"),
            PoorThrows       = GetInt(d.Value, "poor_throws"),
            DefendedPasses   = GetInt(d.Value, "defended_passes"),
            DroppedPasses    = GetInt(d.Value, "dropped_passes"),
            Spikes           = GetInt(d.Value, "spikes"),
            Blitzes          = GetInt(d.Value, "blitzes"),
            Hurries          = GetInt(d.Value, "hurries"),
            Knockdowns       = GetInt(d.Value, "knockdowns"),
            PocketTime       = GetDouble(d.Value, "pocket_time"),
            AvgPocketTime    = GetDoubleN(d.Value, "avg_pocket_time"),
            BattedPasses     = GetInt(d.Value, "batted_passes"),
            OnTargetThrows   = GetInt(d.Value, "on_target_throws"),
            FirstDowns       = GetIntN(d.Value, "first_downs"),
        };

    private static SeasonReceivingStats? MapReceiving(JsonElement? d) =>
        d is null ? null : new SeasonReceivingStats
        {
            Targets           = GetInt(d.Value, "targets"),
            Receptions        = GetInt(d.Value, "receptions"),
            AvgYards          = GetDecimal(d.Value, "avg_yards"),
            Yards             = GetInt(d.Value, "yards"),
            Touchdowns        = GetInt(d.Value, "touchdowns"),
            YardsAfterCatch   = GetInt(d.Value, "yards_after_catch"),
            Longest           = GetInt(d.Value, "longest"),
            LongestTouchdown  = GetInt(d.Value, "longest_touchdown"),
            RedzoneTargets    = GetInt(d.Value, "redzone_targets"),
            AirYards          = GetInt(d.Value, "air_yards"),
            BrokenTackles     = GetInt(d.Value, "broken_tackles"),
            DroppedPasses     = GetInt(d.Value, "dropped_passes"),
            CatchablePasses   = GetInt(d.Value, "catchable_passes"),
            YardsAfterContact = GetInt(d.Value, "yards_after_contact"),
            FirstDowns        = GetIntN(d.Value, "first_downs"),
        };

    private static SeasonDefenseStats? MapDefense(JsonElement? d) =>
        d is null ? null : new SeasonDefenseStats
        {
            Tackles                = GetInt(d.Value, "tackles"),
            Assists                = GetInt(d.Value, "assists"),
            Combined               = GetInt(d.Value, "combined"),
            Sacks                  = GetDecimal(d.Value, "sacks"),
            SackYards              = GetDecimal(d.Value, "sack_yards"),
            Interceptions          = GetInt(d.Value, "interceptions"),
            PassesDefended         = GetInt(d.Value, "passes_defended"),
            ForcedFumbles          = GetInt(d.Value, "forced_fumbles"),
            FumbleRecoveries       = GetInt(d.Value, "fumble_recoveries"),
            QbHits                 = GetInt(d.Value, "qb_hits"),
            Tloss                  = GetDecimal(d.Value, "tloss"),
            TlossYards             = GetDecimal(d.Value, "tloss_yards"),
            Safeties               = GetInt(d.Value, "safeties"),
            SpTackles              = GetInt(d.Value, "sp_tackles"),
            SpAssists              = GetInt(d.Value, "sp_assists"),
            SpForcedFumbles        = GetInt(d.Value, "sp_forced_fumbles"),
            SpFumbleRecoveries     = GetInt(d.Value, "sp_fumble_recoveries"),
            SpBlocks               = GetInt(d.Value, "sp_blocks"),
            MiscTackles            = GetInt(d.Value, "misc_tackles"),
            MiscAssists            = GetInt(d.Value, "misc_assists"),
            MiscForcedFumbles      = GetInt(d.Value, "misc_forced_fumbles"),
            MiscFumbleRecoveries   = GetInt(d.Value, "misc_fumble_recoveries"),
            SpOwnFumbleRecoveries  = GetInt(d.Value, "sp_own_fumble_recoveries"),
            SpOppFumbleRecoveries  = GetInt(d.Value, "sp_opp_fumble_recoveries"),
            ThreeAndOutsForced     = GetInt(d.Value, "three_and_outs_forced"),
            FourthDownStops        = GetInt(d.Value, "fourth_down_stops"),
            DefTargets             = GetInt(d.Value, "def_targets"),
            DefComps               = GetInt(d.Value, "def_comps"),
            Blitzes                = GetInt(d.Value, "blitzes"),
            Hurries                = GetInt(d.Value, "hurries"),
            Knockdowns             = GetInt(d.Value, "knockdowns"),
            MissedTackles          = GetInt(d.Value, "missed_tackles"),
            BattedPasses           = GetInt(d.Value, "batted_passes"),
        };

    private static SeasonFieldGoalStats? MapFieldGoals(JsonElement? d) =>
        d is null ? null : new SeasonFieldGoalStats
        {
            Attempts   = GetInt(d.Value, "attempts"),
            Made       = GetInt(d.Value, "made"),
            Blocked    = GetInt(d.Value, "blocked"),
            Yards      = GetInt(d.Value, "yards"),
            AvgYards   = GetDecimal(d.Value, "avg_yards"),
            Longest    = GetInt(d.Value, "longest"),
            Missed     = GetInt(d.Value, "missed"),
            Pct        = GetDecimal(d.Value, "pct"),
            Attempts19 = GetInt(d.Value, "attempts_19"),
            Attempts29 = GetInt(d.Value, "attempts_29"),
            Attempts39 = GetInt(d.Value, "attempts_39"),
            Attempts49 = GetInt(d.Value, "attempts_49"),
            Attempts50 = GetInt(d.Value, "attempts_50"),
            Made19     = GetInt(d.Value, "made_19"),
            Made29     = GetInt(d.Value, "made_29"),
            Made39     = GetInt(d.Value, "made_39"),
            Made49     = GetInt(d.Value, "made_49"),
            Made50     = GetInt(d.Value, "made_50"),
        };

    private static SeasonPuntStats? MapPunts(JsonElement? d) =>
        d is null ? null : new SeasonPuntStats
        {
            Attempts    = GetInt(d.Value, "attempts"),
            Yards       = GetInt(d.Value, "yards"),
            NetYards    = GetInt(d.Value, "net_yards"),
            Blocked     = GetInt(d.Value, "blocked"),
            Touchbacks  = GetInt(d.Value, "touchbacks"),
            Inside20    = GetInt(d.Value, "inside_20"),
            ReturnYards = GetInt(d.Value, "return_yards"),
            AvgNetYards = GetDecimal(d.Value, "avg_net_yards"),
            AvgYards    = GetDecimal(d.Value, "avg_yards"),
            Longest     = GetInt(d.Value, "longest"),
            HangTime    = GetDouble(d.Value, "hang_time"),
            AvgHangTime = GetDouble(d.Value, "avg_hang_time"),
        };

    private static SeasonKickoffStats? MapKickoffs(JsonElement? d) =>
        d is null ? null : new SeasonKickoffStats
        {
            Kickoffs        = GetInt(d.Value, "number"),
            Endzone         = GetInt(d.Value, "endzone"),
            Inside20        = GetInt(d.Value, "inside_20"),
            ReturnYards     = GetInt(d.Value, "return_yards"),
            Touchbacks      = GetInt(d.Value, "touchbacks"),
            Yards           = GetInt(d.Value, "yards"),
            OutOfBounds     = GetInt(d.Value, "out_of_bounds"),
            OnsideAttempts  = GetInt(d.Value, "onside_attempts"),
            OnsideSuccesses = GetInt(d.Value, "onside_successes"),
            SquibKicks      = GetInt(d.Value, "squib_kicks"),
        };

    private static SeasonPuntReturnStats? MapPuntReturns(JsonElement? d) =>
        d is null ? null : new SeasonPuntReturnStats
        {
            AvgYards         = GetDecimal(d.Value, "avg_yards"),
            Returns          = GetInt(d.Value, "number"),
            Yards            = GetInt(d.Value, "yards"),
            Longest          = GetInt(d.Value, "longest"),
            Touchdowns       = GetInt(d.Value, "touchdowns"),
            LongestTouchdown = GetInt(d.Value, "longest_touchdown"),
            Faircatches      = GetInt(d.Value, "faircatches"),
        };

    private static SeasonKickReturnStats? MapKickReturns(JsonElement? d) =>
        d is null ? null : new SeasonKickReturnStats
        {
            AvgYards         = GetDecimal(d.Value, "avg_yards"),
            Yards            = GetInt(d.Value, "yards"),
            Longest          = GetInt(d.Value, "longest"),
            Touchdowns       = GetInt(d.Value, "touchdowns"),
            LongestTouchdown = GetInt(d.Value, "longest_touchdown"),
            Faircatches      = GetInt(d.Value, "faircatches"),
            Returns          = GetInt(d.Value, "number"),
        };

    private static SeasonIntReturnStats? MapIntReturns(JsonElement? d) =>
        d is null ? null : new SeasonIntReturnStats
        {
            AvgYards         = GetDecimal(d.Value, "avg_yards"),
            Yards            = GetInt(d.Value, "yards"),
            Longest          = GetInt(d.Value, "longest"),
            Touchdowns       = GetInt(d.Value, "touchdowns"),
            LongestTouchdown = GetInt(d.Value, "longest_touchdown"),
            Returns          = GetInt(d.Value, "number"),
        };

    private static SeasonFumbleStats? MapFumbles(JsonElement? d) =>
        d is null ? null : new SeasonFumbleStats
        {
            Fumbles       = GetInt(d.Value, "fumbles"),
            LostFumbles   = GetInt(d.Value, "lost_fumbles"),
            OwnRec        = GetInt(d.Value, "own_rec"),
            OwnRecYards   = GetInt(d.Value, "own_rec_yards"),
            OppRec        = GetInt(d.Value, "opp_rec"),
            OppRecYards   = GetInt(d.Value, "opp_rec_yards"),
            OutOfBounds   = GetInt(d.Value, "out_of_bounds"),
            ForcedFumbles = GetInt(d.Value, "forced_fumbles"),
            OwnRecTds     = GetInt(d.Value, "own_rec_tds"),
            OppRecTds     = GetInt(d.Value, "opp_rec_tds"),
            EzRecTds      = GetInt(d.Value, "ez_rec_tds"),
        };

    private static SeasonPenaltyStats? MapPenalties(JsonElement? d) =>
        d is null ? null : new SeasonPenaltyStats
        {
            Penalties  = GetInt(d.Value, "penalties"),
            Yards      = GetInt(d.Value, "yards"),
            FirstDowns = GetIntN(d.Value, "first_downs"),
        };

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static JsonElement? Totals(JsonElement team, string category)
    {
        if (!team.TryGetProperty(category, out var cat)) return null;
        if (!cat.TryGetProperty("totals", out var totals)) return null;
        return totals;
    }

    private static Guid GetGuid(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v)) return Guid.Empty;
        if (v.TryGetGuid(out var g)) return g;
        return Guid.TryParse(v.GetString(), out var p) ? p : Guid.Empty;
    }

    private static int GetInt(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v)) return 0;
        return v.TryGetInt32(out var i) ? i : (v.TryGetDouble(out var d) ? (int)d : 0);
    }

    private static int? GetIntN(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v) || v.ValueKind == JsonValueKind.Null) return null;
        return v.TryGetInt32(out var i) ? i : (v.TryGetDouble(out var d) ? (int)d : null);
    }

    private static double GetDouble(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v)) return 0;
        return v.TryGetDouble(out var d) ? d : 0;
    }

    private static double? GetDoubleN(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v) || v.ValueKind == JsonValueKind.Null) return null;
        return v.TryGetDouble(out var d) ? d : null;
    }

    private static decimal GetDecimal(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v)) return 0;
        return v.TryGetDecimal(out var d) ? d : 0;
    }

    private static string? GetStrN(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v) || v.ValueKind == JsonValueKind.Null) return null;
        var s = v.GetString();
        return string.IsNullOrWhiteSpace(s) ? null : s;
    }
}
