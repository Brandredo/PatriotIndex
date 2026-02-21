using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum EventType
{
    [EnumMember(Value = "setup")]

    Setup,
    [EnumMember(Value = "timeout")]

    Timeout,
    [EnumMember(Value = "tv_timeout")]

    TvTimeout,
    [EnumMember(Value = "comment")]

    TwoMinuteWarning,
    [EnumMember(Value = "period_end")]

    Comment,
    PeriodEnd,
    [EnumMember(Value = "game_over")]

    GameOver
}