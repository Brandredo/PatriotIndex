using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum SeasonType
{
    [EnumMember(Value = "PRE")]
    Preseason,
    [EnumMember(Value = "REG")]
    Regular,
    [EnumMember(Value = "PST")]
    Playoffs
}