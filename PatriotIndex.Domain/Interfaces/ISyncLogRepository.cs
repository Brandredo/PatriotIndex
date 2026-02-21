using PatriotIndex.Domain.DTOs;

namespace PatriotIndex.Domain.Interfaces;

public interface ISyncLogRepository
{
    Task<IReadOnlyList<SyncLogDto>> GetRecentAsync(int limit);
}
