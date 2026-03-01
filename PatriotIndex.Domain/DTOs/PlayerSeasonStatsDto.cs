namespace PatriotIndex.Domain.DTOs;

public record PlayerSeasonStatsDto(
    Guid Id,
    int SeasonYear,
    string SeasonType,
    int GamesPlayed,
    int GamesStarted,
    StatBlockDto Stats);