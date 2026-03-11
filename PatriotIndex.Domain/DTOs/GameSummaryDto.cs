namespace PatriotIndex.Domain.DTOs;

public record GameSummaryDto(
    Guid Id,
    DateTime? Scheduled,
    string? Status,
    string? SeasonType,
    int? SeasonYear,
    string? WeekTitle,
    int? WeekSequence,
    Guid? HomeTeamId,
    string? HomeTeamAlias,
    string? HomeTeamMarket,
    string? HomeTeamName,
    int? HomePoints,
    Guid? AwayTeamId,
    string? AwayTeamAlias,
    string? AwayTeamMarket,
    string? AwayTeamName,
    int? AwayPoints);
