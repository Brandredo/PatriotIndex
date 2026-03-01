using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum PlayType
{
    [EnumMember(Value = "pass")]
    Pass,

    [EnumMember(Value = "rush")]
    Rush,

    [EnumMember(Value = "faircatch_kick")]
    FairCatchKick,

    [EnumMember(Value = "extra_point")]
    ExtraPoint,

    [EnumMember(Value = "conversion")]
    Conversion,

    [EnumMember(Value = "free_kick")]
    FreeKick,

    [EnumMember(Value = "kickoff")]
    Kickoff,

    [EnumMember(Value = "punt")]
    Punt,

    [EnumMember(Value = "field_goal")]
    FieldGoal,

    [EnumMember(Value = "penalty")]
    Penalty
}