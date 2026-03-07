using System.Text.Json.Serialization;

namespace PatriotIndex.Domain.DTOs;

// ── Touchdowns ────────────────────────────────────────────────────────────

public record SeasonTouchdownsDto(
    [property: JsonPropertyName("pass")] int Pass,
    [property: JsonPropertyName("rush")] int Rush,
    [property: JsonPropertyName("total_return")] int TotalReturn,
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("fumble_return")] int FumbleReturn,
    [property: JsonPropertyName("int_return")] int IntReturn,
    [property: JsonPropertyName("kick_return")] int KickReturn,
    [property: JsonPropertyName("punt_return")] int PuntReturn,
    [property: JsonPropertyName("other")] int Other
);

// ── Rushing ───────────────────────────────────────────────────────────────

public record SeasonRushingDto(
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("attempts")] int Attempts,
    [property: JsonPropertyName("touchdowns")] int Touchdowns,
    [property: JsonPropertyName("tlost")] int Tlost,
    [property: JsonPropertyName("tlost_yards")] int TlostYards,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("longest_touchdown")] int LongestTouchdown,
    [property: JsonPropertyName("redzone_attempts")] int RedzoneAttempts,
    [property: JsonPropertyName("broken_tackles")] int BrokenTackles,
    [property: JsonPropertyName("kneel_downs")] int KneelDowns,
    [property: JsonPropertyName("scrambles")] int Scrambles,
    [property: JsonPropertyName("yards_after_contact")] int YardsAfterContact,
    // player-only
    [property: JsonPropertyName("first_downs")] int? FirstDowns
);

// ── Passing ───────────────────────────────────────────────────────────────

public record SeasonPassingDto(
    [property: JsonPropertyName("attempts")] int Attempts,
    [property: JsonPropertyName("completions")] int Completions,
    [property: JsonPropertyName("cmp_pct")] double CmpPct,
    [property: JsonPropertyName("interceptions")] int Interceptions,
    [property: JsonPropertyName("sack_yards")] int SackYards,
    [property: JsonPropertyName("rating")] double Rating,
    [property: JsonPropertyName("touchdowns")] int Touchdowns,
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("sacks")] int Sacks,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("longest_touchdown")] int LongestTouchdown,
    [property: JsonPropertyName("air_yards")] int AirYards,
    [property: JsonPropertyName("redzone_attempts")] int RedzoneAttempts,
    [property: JsonPropertyName("net_yards")] int NetYards,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("gross_yards")] int GrossYards,
    [property: JsonPropertyName("int_touchdowns")] int IntTouchdowns,
    [property: JsonPropertyName("throw_aways")] int ThrowAways,
    [property: JsonPropertyName("poor_throws")] int PoorThrows,
    [property: JsonPropertyName("defended_passes")] int DefendedPasses,
    [property: JsonPropertyName("dropped_passes")] int DroppedPasses,
    [property: JsonPropertyName("spikes")] int Spikes,
    [property: JsonPropertyName("blitzes")] int Blitzes,
    [property: JsonPropertyName("hurries")] int Hurries,
    [property: JsonPropertyName("knockdowns")] int Knockdowns,
    [property: JsonPropertyName("pocket_time")] double PocketTime,
    [property: JsonPropertyName("batted_passes")] int BattedPasses,
    [property: JsonPropertyName("on_target_throws")] int OnTargetThrows,
    // player-only
    [property: JsonPropertyName("first_downs")] int? FirstDowns,
    [property: JsonPropertyName("avg_pocket_time")] double? AvgPocketTime
);

// ── Receiving ─────────────────────────────────────────────────────────────

public record SeasonReceivingDto(
    [property: JsonPropertyName("targets")] int Targets,
    [property: JsonPropertyName("receptions")] int Receptions,
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("touchdowns")] int Touchdowns,
    [property: JsonPropertyName("yards_after_catch")] int YardsAfterCatch,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("longest_touchdown")] int LongestTouchdown,
    [property: JsonPropertyName("redzone_targets")] int RedzoneTargets,
    [property: JsonPropertyName("air_yards")] int AirYards,
    [property: JsonPropertyName("broken_tackles")] int BrokenTackles,
    [property: JsonPropertyName("dropped_passes")] int DroppedPasses,
    [property: JsonPropertyName("catchable_passes")] int CatchablePasses,
    [property: JsonPropertyName("yards_after_contact")] int YardsAfterContact,
    // player-only
    [property: JsonPropertyName("first_downs")] int? FirstDowns
);

// ── Defense ───────────────────────────────────────────────────────────────

public record SeasonDefenseDto(
    [property: JsonPropertyName("tackles")] int Tackles,
    [property: JsonPropertyName("assists")] int Assists,
    [property: JsonPropertyName("combined")] int Combined,
    [property: JsonPropertyName("sacks")] double Sacks,
    [property: JsonPropertyName("sack_yards")] int SackYards,
    [property: JsonPropertyName("interceptions")] int Interceptions,
    [property: JsonPropertyName("passes_defended")] int PassesDefended,
    [property: JsonPropertyName("forced_fumbles")] int ForcedFumbles,
    [property: JsonPropertyName("fumble_recoveries")] int FumbleRecoveries,
    [property: JsonPropertyName("qb_hits")] int QbHits,
    [property: JsonPropertyName("tloss")] int Tloss,
    [property: JsonPropertyName("tloss_yards")] int TlossYards,
    [property: JsonPropertyName("safeties")] int Safeties,
    [property: JsonPropertyName("sp_tackles")] int SpTackles,
    [property: JsonPropertyName("sp_assists")] int SpAssists,
    [property: JsonPropertyName("sp_forced_fumbles")] int SpForcedFumbles,
    [property: JsonPropertyName("sp_fumble_recoveries")] int SpFumbleRecoveries,
    [property: JsonPropertyName("sp_blocks")] int SpBlocks,
    [property: JsonPropertyName("misc_tackles")] int MiscTackles,
    [property: JsonPropertyName("misc_assists")] int MiscAssists,
    [property: JsonPropertyName("misc_forced_fumbles")] int MiscForcedFumbles,
    [property: JsonPropertyName("misc_fumble_recoveries")] int MiscFumbleRecoveries,
    [property: JsonPropertyName("sp_own_fumble_recoveries")] int SpOwnFumbleRecoveries,
    [property: JsonPropertyName("sp_opp_fumble_recoveries")] int SpOppFumbleRecoveries,
    [property: JsonPropertyName("three_and_outs_forced")] int ThreeAndOutsForced,
    [property: JsonPropertyName("fourth_down_stops")] int FourthDownStops,
    [property: JsonPropertyName("def_targets")] int DefTargets,
    [property: JsonPropertyName("def_comps")] int DefComps,
    [property: JsonPropertyName("blitzes")] int Blitzes,
    [property: JsonPropertyName("hurries")] int Hurries,
    [property: JsonPropertyName("knockdowns")] int Knockdowns,
    [property: JsonPropertyName("missed_tackles")] int MissedTackles,
    [property: JsonPropertyName("batted_passes")] int BattedPasses
);

// ── Field Goals ───────────────────────────────────────────────────────────

public record SeasonFieldGoalsDto(
    [property: JsonPropertyName("attempts")] int Attempts,
    [property: JsonPropertyName("made")] int Made,
    [property: JsonPropertyName("blocked")] int Blocked,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("missed")] int Missed,
    [property: JsonPropertyName("pct")] double Pct,
    [property: JsonPropertyName("attempts_19")] int Attempts19,
    [property: JsonPropertyName("attempts_29")] int Attempts29,
    [property: JsonPropertyName("attempts_39")] int Attempts39,
    [property: JsonPropertyName("attempts_49")] int Attempts49,
    [property: JsonPropertyName("attempts_50")] int Attempts50,
    [property: JsonPropertyName("made_19")] int Made19,
    [property: JsonPropertyName("made_29")] int Made29,
    [property: JsonPropertyName("made_39")] int Made39,
    [property: JsonPropertyName("made_49")] int Made49,
    [property: JsonPropertyName("made_50")] int Made50
);

// ── Kickoffs ──────────────────────────────────────────────────────────────

public record SeasonKickoffsDto(
    [property: JsonPropertyName("kickoffs")] int Kickoffs,
    [property: JsonPropertyName("endzone")] int Endzone,
    [property: JsonPropertyName("inside_20")] int Inside20,
    [property: JsonPropertyName("return_yards")] int ReturnYards,
    [property: JsonPropertyName("returned")] int Returned,
    [property: JsonPropertyName("touchbacks")] int Touchbacks,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("out_of_bounds")] int OutOfBounds,
    [property: JsonPropertyName("onside_attempts")] int OnsideAttempts,
    [property: JsonPropertyName("onside_successes")] int OnsideSuccesses,
    [property: JsonPropertyName("squib_kicks")] int SquibKicks
);

// ── Kick Returns ──────────────────────────────────────────────────────────

public record SeasonKickReturnsDto(
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("touchdowns")] int Touchdowns,
    [property: JsonPropertyName("longest_touchdown")] int LongestTouchdown,
    [property: JsonPropertyName("faircatches")] int Faircatches,
    [property: JsonPropertyName("returns")] int Returns
);

// ── Punts ─────────────────────────────────────────────────────────────────

public record SeasonPuntsDto(
    [property: JsonPropertyName("attempts")] int Attempts,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("net_yards")] int NetYards,
    [property: JsonPropertyName("blocked")] int Blocked,
    [property: JsonPropertyName("touchbacks")] int Touchbacks,
    [property: JsonPropertyName("inside_20")] int Inside20,
    [property: JsonPropertyName("return_yards")] int ReturnYards,
    [property: JsonPropertyName("avg_net_yards")] double AvgNetYards,
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("hang_time")] double HangTime,
    [property: JsonPropertyName("avg_hang_time")] double AvgHangTime
);

// ── Punt Returns ──────────────────────────────────────────────────────────

public record SeasonPuntReturnsDto(
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("returns")] int Returns,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("touchdowns")] int Touchdowns,
    [property: JsonPropertyName("longest_touchdown")] int LongestTouchdown,
    [property: JsonPropertyName("faircatches")] int Faircatches
);

// ── Interceptions ─────────────────────────────────────────────────────────

public record SeasonInterceptionsDto(
    [property: JsonPropertyName("return_yards")] int ReturnYards,
    [property: JsonPropertyName("returned")] int Returned,
    [property: JsonPropertyName("interceptions")] int Interceptions
);

// ── Int Returns ───────────────────────────────────────────────────────────

public record SeasonIntReturnsDto(
    [property: JsonPropertyName("avg_yards")] double AvgYards,
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("longest")] int Longest,
    [property: JsonPropertyName("touchdowns")] int Touchdowns,
    [property: JsonPropertyName("longest_touchdown")] int LongestTouchdown,
    [property: JsonPropertyName("returns")] int Returns
);

// ── Fumbles ───────────────────────────────────────────────────────────────

public record SeasonFumblesDto(
    [property: JsonPropertyName("fumbles")] int Fumbles,
    [property: JsonPropertyName("lost_fumbles")] int LostFumbles,
    [property: JsonPropertyName("own_rec")] int OwnRec,
    [property: JsonPropertyName("own_rec_yards")] int OwnRecYards,
    [property: JsonPropertyName("opp_rec")] int OppRec,
    [property: JsonPropertyName("opp_rec_yards")] int OppRecYards,
    [property: JsonPropertyName("out_of_bounds")] int OutOfBounds,
    [property: JsonPropertyName("forced_fumbles")] int ForcedFumbles,
    [property: JsonPropertyName("own_rec_tds")] int OwnRecTds,
    [property: JsonPropertyName("opp_rec_tds")] int OppRecTds,
    [property: JsonPropertyName("ez_rec_tds")] int EzRecTds
);

// ── First Downs ───────────────────────────────────────────────────────────

public record SeasonFirstDownsDto(
    [property: JsonPropertyName("pass")] int Pass,
    [property: JsonPropertyName("penalty")] int Penalty,
    [property: JsonPropertyName("rush")] int Rush,
    [property: JsonPropertyName("total")] int Total
);

// ── Penalties ─────────────────────────────────────────────────────────────

public record SeasonPenaltiesDto(
    [property: JsonPropertyName("penalties")] int Penalties,
    [property: JsonPropertyName("yards")] int Yards,
    // player-only
    [property: JsonPropertyName("first_downs")] int? FirstDowns
);

// ── Misc Returns ──────────────────────────────────────────────────────────

public record SeasonMiscReturnsDto(
    [property: JsonPropertyName("yards")] int Yards,
    [property: JsonPropertyName("touchdowns")] int Touchdowns,
    [property: JsonPropertyName("longest_touchdown")] int LongestTouchdown,
    [property: JsonPropertyName("blk_fg_touchdowns")] int BlkFgTouchdowns,
    [property: JsonPropertyName("blk_punt_touchdowns")] int BlkPuntTouchdowns,
    [property: JsonPropertyName("fg_return_touchdowns")] int FgReturnTouchdowns,
    [property: JsonPropertyName("ez_rec_touchdowns")] int EzRecTouchdowns,
    [property: JsonPropertyName("returns")] int Returns
);

// ── Extra Points (Team) ───────────────────────────────────────────────────

public record SeasonTeamExtraPointsDto(
    [property: JsonPropertyName("kicks")] SeasonEpKicksDto? Kicks,
    [property: JsonPropertyName("conversions")] SeasonEpConversionsDto? Conversions
);

public record SeasonEpKicksDto(
    [property: JsonPropertyName("attempts")] int Attempts,
    [property: JsonPropertyName("blocked")] int Blocked,
    [property: JsonPropertyName("made")] int Made,
    [property: JsonPropertyName("pct")] double Pct
);

public record SeasonEpConversionsDto(
    [property: JsonPropertyName("pass_attempts")] int PassAttempts,
    [property: JsonPropertyName("pass_successes")] int PassSuccesses,
    [property: JsonPropertyName("rush_attempts")] int RushAttempts,
    [property: JsonPropertyName("rush_successes")] int RushSuccesses,
    [property: JsonPropertyName("defense_attempts")] int DefenseAttempts,
    [property: JsonPropertyName("defense_successes")] int DefenseSuccesses,
    [property: JsonPropertyName("turnover_successes")] int TurnoverSuccesses
);

// ── Extra Points (Player / Kicker — flat structure) ───────────────────────

public record SeasonPlayerExtraPointsDto(
    [property: JsonPropertyName("attempts")] int Attempts,
    [property: JsonPropertyName("made")] int Made,
    [property: JsonPropertyName("blocked")] int Blocked,
    [property: JsonPropertyName("missed")] int Missed,
    [property: JsonPropertyName("pct")] double Pct
);

// ── Efficiency ────────────────────────────────────────────────────────────

public record SeasonEfficiencyDto(
    [property: JsonPropertyName("goaltogo")] SeasonEfficiencyBlockDto? Goaltogo,
    [property: JsonPropertyName("redzone")] SeasonEfficiencyBlockDto? Redzone,
    [property: JsonPropertyName("thirddown")] SeasonEfficiencyBlockDto? Thirddown,
    [property: JsonPropertyName("fourthdown")] SeasonEfficiencyBlockDto? Fourthdown
);

public record SeasonEfficiencyBlockDto(
    [property: JsonPropertyName("attempts")] int Attempts,
    [property: JsonPropertyName("successes")] int Successes,
    [property: JsonPropertyName("pct")] double Pct
);
