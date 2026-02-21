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
    [EnumMember(Value = "two_minute_warning")]
    TwoMinuteWarning,
    [EnumMember(Value = "comment")]
    Comment,
    [EnumMember(Value = "period_end")]
    PeriodEnd,
    [EnumMember(Value = "game_over")]
    GameOver
}