namespace PatriotIndex.Domain.DTOs;

public record ConferenceWithDivisionsDto(
    Guid Id,
    string Name,
    string Alias,
    IReadOnlyList<DivisionWithTeamsDto> Divisions);

public record DivisionWithTeamsDto(
    Guid Id,
    string Name,
    string Alias,
    IReadOnlyList<TeamSummaryDto> Teams);
