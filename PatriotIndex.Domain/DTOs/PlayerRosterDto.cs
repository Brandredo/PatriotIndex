namespace PatriotIndex.Domain.DTOs;

public record PlayerRosterDto(
    Guid Id,
    string? Name,
    string? FirstName,
    string? LastName,
    string? Jersey,
    string? Position,
    string? Status,
    int? Experience);