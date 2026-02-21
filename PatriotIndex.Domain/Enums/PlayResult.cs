using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum PlayResult
{
    [EnumMember(Value = "accepted")]
    Accepted,

    [EnumMember(Value = "backward pass")]
    BackwardPass,

    [EnumMember(Value = "blocked")]
    Blocked,

    [EnumMember(Value = "declined")]
    Declined,

    [EnumMember(Value = "downed")]
    Downed,

    [EnumMember(Value = "fair catch")]
    FairCatch,

    [EnumMember(Value = "fumbled")]
    Fumbled,

    [EnumMember(Value = "good")]
    Good,

    [EnumMember(Value = "missed")]
    Missed,

    [EnumMember(Value = "muffed")]
    Muffed,

    [EnumMember(Value = "no good")]
    NoGood,

    [EnumMember(Value = "nullified touchdown")]
    NullifiedTouchdown,

    [EnumMember(Value = "offset")]
    Offset,

    [EnumMember(Value = "overturned")]
    Overturned,

    [EnumMember(Value = "pass")]
    Pass,

    [EnumMember(Value = "penalty")]
    Penalty,

    [EnumMember(Value = "pushed out of bounds")]
    PushedOutOfBounds,

    [EnumMember(Value = "ran out of bounds")]
    RanOutOfBounds,

    [EnumMember(Value = "returned")]
    Returned,

    [EnumMember(Value = "rush")]
    Rush,

    [EnumMember(Value = "tackled")]
    Tackled,

    [EnumMember(Value = "touchback")]
    Touchback,

    [EnumMember(Value = "touchdown")]
    Touchdown,
}