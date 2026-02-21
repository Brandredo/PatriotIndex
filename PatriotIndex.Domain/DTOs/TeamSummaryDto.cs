using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.DTOs;

public record TeamSummaryDto(
    Guid Id, string Name, string Market, string Alias,
    TeamColors? Colors,
    DivisionSummaryDto? Division);