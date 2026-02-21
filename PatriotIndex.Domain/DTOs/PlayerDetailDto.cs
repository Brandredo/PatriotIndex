namespace PatriotIndex.Domain.DTOs;

public record PlayerDetailDto(
    Guid Id, string? Name, string? FirstName, string? LastName,
    string? Jersey, string? Position, string? Status, int? Experience,
    int? Height, int? Weight, string? BirthDate, string? College, int? RookieYear,
    long? Salary, string? SrId,
    int? DraftYear, int? DraftRound, int? DraftPick, string? DraftTeam,
    TeamSummaryDto? Team,
    IReadOnlyList<PlayerSeasonStatsDto> SeasonStats);