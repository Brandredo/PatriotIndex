namespace PatriotIndex.Domain.DTOs;

public record PlayStatDto(
    Guid Id,
    string StatType,
    Guid? PlayerId,
    string? PlayerName,
    Guid? TeamId,
    string? TeamAlias,
    int? Yards,
    int? Attempt,
    int? Complete,
    int? Touchdown,
    int? Interception,
    int? Fumble,
    int? Sack,
    int? Touchback,
    string? Category);