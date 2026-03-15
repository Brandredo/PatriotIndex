namespace PatriotIndex.Domain.DTOs;

public record TeamPlayerStatsDto(
    Guid PlayerId,
    string? PlayerName,
    string? Jersey,
    string? Position,
    int GamesPlayed,
    int GamesStarted,
    StatBlockDto Stats);
