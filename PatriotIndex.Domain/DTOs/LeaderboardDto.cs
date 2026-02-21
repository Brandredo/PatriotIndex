namespace PatriotIndex.Domain.DTOs;

public record LeaderboardDto(
    string Category, int SeasonYear, string SeasonType,
    string? PositionFilter,
    IReadOnlyList<LeaderboardEntryDto> Leaders);