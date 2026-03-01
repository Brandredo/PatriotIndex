namespace PatriotIndex.Domain.DTOs;

public record PlayerGameLogDto(
    Guid GameId,
    DateTime? Scheduled,
    string? Opponent,
    bool IsHome,
    int? HomePoints,
    int? AwayPoints,
    StatBlockDto Stats);