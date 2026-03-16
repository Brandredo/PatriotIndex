namespace PatriotIndex.Domain.DTOs;

public record PagedResultDto<T>(T[] Items, string? NextCursor, int TotalCount);
