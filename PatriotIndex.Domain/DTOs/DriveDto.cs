namespace PatriotIndex.Domain.DTOs;

public record DriveDto(
    Guid Id,
    int? PeriodNumber,
    int? Sequence,
    string? StartReason,
    string? EndReason,
    int? PlayCount,
    string? Duration,
    int? FirstDowns,
    int? NetYards,
    string? StartClock,
    string? EndClock,
    Guid? OffensiveTeamId,
    string? OffensiveTeamAlias,
    Guid? DefensiveTeamId,
    string? DefensiveTeamAlias,
    int OffensivePoints,
    int DefensivePoints,
    IReadOnlyList<PlayDto> Plays);