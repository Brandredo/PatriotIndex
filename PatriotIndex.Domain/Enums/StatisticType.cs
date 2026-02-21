using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum StatisticType
{
    [EnumMember(Value = "pass")]
    Pass,

    [EnumMember(Value = "receive")]
    Receive,

    [EnumMember(Value = "defense")]
    Defense,

    [EnumMember(Value = "rush")]
    Rush,

    [EnumMember(Value = "punt")]
    Punt,

    [EnumMember(Value = "return")]
    Return,

    [EnumMember(Value = "first_down")]
    FirstDown,

    [EnumMember(Value = "extra_point")]
    ExtraPoint,

    [EnumMember(Value = "kick")]
    Kick,

    [EnumMember(Value = "penalty")]
    Penalty,

    [EnumMember(Value = "field_goal")]
    FieldGoal,

    [EnumMember(Value = "fumble")]
    Fumble,

    [EnumMember(Value = "defense_conversion")]
    DefenseConversion,

    [EnumMember(Value = "blocked")]
    Blocked,
}