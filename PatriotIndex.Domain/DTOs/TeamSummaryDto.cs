using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.DTOs;

public record TeamSummaryDto(
    Guid Id,
    string Name,
    string Market,
    string Alias,
    TeamColors? Colors,
    DivisionSummaryDto? Division);
    
    
public record PlayerMinDto(Guid Id, string? Name);

public record TeamSummaryWithRosterDto(
    Guid Id,
    string Name,
    string Market,
    string Alias,
    IReadOnlyList<PlayerMinDto> Roster);