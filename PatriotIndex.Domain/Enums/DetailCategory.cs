using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum DetailCategory
{
    [EnumMember(Value = "aborted_snap")]
    AbortedSnap,

    [EnumMember(Value = "aborted_snap_recovery")]
    AbortedSnapRecovery,

    [EnumMember(Value = "blocked_kick")]
    BlockedKick,

    [EnumMember(Value = "blocked_kick_recovery")]
    BlockedKickRecovery,

    [EnumMember(Value = "defensive_conversion_fumble_recovery")]
    DefensiveConversionFumbleRecovery,

    [EnumMember(Value = "defensive_conversion_interception_recovery")]
    DefensiveConversionInterceptionRecovery,

    [EnumMember(Value = "defensive_conversion_recovery")]
    DefensiveConversionRecovery,

    [EnumMember(Value = "downed")]
    Downed,

    [EnumMember(Value = "extra_point_attempt")]
    ExtraPointAttempt,

    [EnumMember(Value = "fair_catch")]
    FairCatch,

    [EnumMember(Value = "field_goal")]
    FieldGoal,

    [EnumMember(Value = "field_goal_return")]
    FieldGoalReturn,

    [EnumMember(Value = "first_down")]
    FirstDown,

    [EnumMember(Value = "forced_fumble")]
    ForcedFumble,

    [EnumMember(Value = "fourth_down_conversion")]
    FourthDownConversion,

    [EnumMember(Value = "fumble")]
    Fumble,

    [EnumMember(Value = "fumble_out_of_bounds")]
    FumbleOutOfBounds,

    [EnumMember(Value = "kick_off")]
    KickOff,

    [EnumMember(Value = "kick_off_return")]
    KickOffReturn,

    [EnumMember(Value = "kneel")]
    Kneel,

    [EnumMember(Value = "lateral")]
    Lateral,

    [EnumMember(Value = "muffed_kick")]
    MuffedKick,

    [EnumMember(Value = "muffed_punt")]
    MuffedPunt,

    [EnumMember(Value = "no_play")]
    NoPlay,

    [EnumMember(Value = "onside_kick_off")]
    OnsideKickOff,

    [EnumMember(Value = "onside_kick_off_recovery")]
    OnsideKickOffRecovery,

    [EnumMember(Value = "opponent_aborted_snap_recovery")]
    OpponentAbortedSnapRecovery,

    [EnumMember(Value = "opponent_blocked_kick_recovery")]
    OpponentBlockedKickRecovery,

    [EnumMember(Value = "opponent_fumble_recovery")]
    OpponentFumbleRecovery,

    [EnumMember(Value = "out_of_bounds")]
    OutOfBounds,

    [EnumMember(Value = "own_aborted_snap_recovery")]
    OwnAbortedSnapRecovery,

    [EnumMember(Value = "own_blocked_kick_recovery")]
    OwnBlockedKickRecovery,

    [EnumMember(Value = "own_fumble_recovery")]
    OwnFumbleRecovery,

    [EnumMember(Value = "pass")]
    Pass,

    [EnumMember(Value = "pass_completion")]
    PassCompletion,

    [EnumMember(Value = "pass_incompletion")]
    PassIncompletion,

    [EnumMember(Value = "pass_interception")]
    PassInterception,

    [EnumMember(Value = "pass_interception_return")]
    PassInterceptionReturn,

    [EnumMember(Value = "pass_reception")]
    PassReception,

    [EnumMember(Value = "penalty")]
    Penalty,

    [EnumMember(Value = "punt")]
    Punt,

    [EnumMember(Value = "punt_return")]
    PuntReturn,

    [EnumMember(Value = "pushed_out_of_bounds")]
    PushedOutOfBounds,

    [EnumMember(Value = "ran_out_of_bounds")]
    RanOutOfBounds,

    [EnumMember(Value = "review")]
    Review,

    [EnumMember(Value = "review_pending")]
    ReviewPending,

    [EnumMember(Value = "rush")]
    Rush,

    [EnumMember(Value = "sack")]
    Sack,

    [EnumMember(Value = "safety")]
    Safety,

    [EnumMember(Value = "scramble")]
    Scramble,

    [EnumMember(Value = "spike")]
    Spike,

    [EnumMember(Value = "stat_correction")]
    StatCorrection,

    [EnumMember(Value = "tackle")]
    Tackle,

    [EnumMember(Value = "team_timeout")]
    TeamTimeout,

    [EnumMember(Value = "third_down_conversion")]
    ThirdDownConversion,

    [EnumMember(Value = "touchback")]
    Touchback,

    [EnumMember(Value = "touchdown")]
    Touchdown,

    [EnumMember(Value = "two_point_attempt")]
    TwoPointAttempt,

    [EnumMember(Value = "two_point_pass")]
    TwoPointPass,

    [EnumMember(Value = "two_point_return")]
    TwoPointReturn,

    [EnumMember(Value = "two_point_result")]
    TwoPointResult,

    [EnumMember(Value = "two_point_rush")]
    TwoPointRush,
}