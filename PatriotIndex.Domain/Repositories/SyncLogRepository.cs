using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Repositories;

public class SyncLogRepository(PatriotIndexDbContext db) : ISyncLogRepository
{
    public async Task<IReadOnlyList<SyncLogDto>> GetRecentAsync(int limit)
    {
        var logs = await db.SyncLogs
            .OrderByDescending(s => s.StartedAt)
            .Take(limit)
            .ToListAsync();

        return logs.Select(s => new SyncLogDto(
            s.Id, s.EntityType, s.StartedAt, s.CompletedAt,
            s.Status, s.RecordCount, s.ErrorMessage)).ToList();
    }
}
