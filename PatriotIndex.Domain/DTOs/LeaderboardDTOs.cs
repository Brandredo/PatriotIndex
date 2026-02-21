namespace PatriotIndex.Domain.DTOs;

public record LeaderboardEntryDto(
    Guid PlayerId, string? PlayerName, string? Position,
    Guid? TeamId, string? TeamAlias, string? TeamMarket,
    int SeasonYear, string SeasonType,
    double Value);

public record LeaderboardDto(
    string Category, int SeasonYear, string SeasonType,
    string? PositionFilter,
    IReadOnlyList<LeaderboardEntryDto> Leaders);
