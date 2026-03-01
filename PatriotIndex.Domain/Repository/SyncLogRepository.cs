using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class SyncLogRepository(PatriotIndexDbContext ctx)
{
    public async Task InsertEntry(SyncLog entry, CancellationToken cancellationToken)
    {
        await ctx.SyncLogs.AddAsync(entry, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);
    }
}