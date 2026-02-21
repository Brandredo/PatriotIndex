using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Domain.DTOs;

public record PlayerSummaryDto(
    Guid Id, string? Name, string? FirstName, string? LastName,
    string? Jersey, PlayerPosition? Position, string? Status,
    TeamSummaryDto? Team);