namespace PatriotIndex.Domain.DTOs;

public record PlayerSummaryDto(
    Guid Id, string? Name, string? FirstName, string? LastName,
    string? Jersey, string? Position, string? Status,
    TeamSummaryDto? Team);