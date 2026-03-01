namespace PatriotIndex.Domain.DTOs;

public record GamePbpDto(
    Guid GameId,
    string? Title,
    string? Status,
    Guid? HomeTeamId,
    string? HomeTeamAlias,
    int? HomePoints,
    Guid? AwayTeamId,
    string? AwayTeamAlias,
    int? AwayPoints,
    IReadOnlyList<DriveDto> Drives);