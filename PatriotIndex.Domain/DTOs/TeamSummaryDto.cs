namespace PatriotIndex.Domain.DTOs;

public record TeamSummaryDto(
    Guid Id, string Name, string Market, string Alias,
    string? PrimaryColor, string? SecondaryColor,
    DivisionSummaryDto? Division);