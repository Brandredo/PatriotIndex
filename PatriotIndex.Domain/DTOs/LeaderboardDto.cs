using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Domain.DTOs;

public record LeaderboardDto(
    string Category, int SeasonYear, string SeasonType,
    PlayerPosition? PositionFilter,
    IReadOnlyList<LeaderboardEntryDto> Leaders);