namespace PatriotIndex.Domain.DTOs;

public record SyncLogDto(
    Guid Id, string EntityType, DateTime StartedAt, DateTime? CompletedAt,
    string Status, int RecordCount, string? ErrorMessage);
