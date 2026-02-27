using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class SyncLogRepository(IDbContextFactory<PatriotIndexDbContext> dbContextFactory)
{

    public async Task InsertEntry(SyncLog entry, CancellationToken cancellationToken)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await db.SyncLogs.AddAsync(entry, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
    
}