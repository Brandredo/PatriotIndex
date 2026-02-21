namespace PatriotIndex.Domain.DTOs;

public record DivisionSummaryDto(Guid Id, string Name, string Alias, ConferenceSummaryDto Conference);