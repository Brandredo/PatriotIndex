using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum GameStatus
{
    [EnumMember(Value = "scheduled")]
    Scheduled,

    [EnumMember(Value = "created")]
    Created,

    [EnumMember(Value = "inprogress")]
    InProgress,

    [EnumMember(Value = "halftime")]
    Halftime,

    [EnumMember(Value = "complete")]
    Complete,

    [EnumMember(Value = "closed")]
    Closed,

    [EnumMember(Value = "cancelled")]
    Cancelled,

    [EnumMember(Value = "postponed")]
    Postponed,

    [EnumMember(Value = "delayed")]
    Delayed,

    [EnumMember(Value = "suspended")]
    Suspended,

    [EnumMember(Value = "flex-schedule")]
    Flexed,

    [EnumMember(Value = "time-tbd")]
    Tbd
}