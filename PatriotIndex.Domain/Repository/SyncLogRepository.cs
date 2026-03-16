using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class SyncLogRepository(PatriotIndexDbContext ctx)
{
    
    // check if an entry with the same entity type and id already exists from the last 24 hours
    public async Task<JsonDocument?> IsDuplicateEntry(string entityId, CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddHours(-24);

        return await ctx.SyncLogs
            .AsNoTracking()
            .Where(e => e.EntityType == entityId && e.CompletedAt >= cutoff && e.Status == "Success" && e.RawResponse != null)
            .OrderByDescending(e => e.CompletedAt)
            .Select(e => e.RawResponse)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    
    public async Task<long> InsertEntry(SyncLog entry, CancellationToken cancellationToken)
    {
        await ctx.SyncLogs.AddAsync(entry, cancellationToken);
        await ctx.SaveChangesAsync(cancellationToken);

        return entry.Id; // EF Core populates this after save
    }
    
    public async Task<SyncLog?> GetEntry(Guid id, CancellationToken cancellationToken)
    {
        return await ctx.SyncLogs.FindAsync([id], cancellationToken);
    }

    public async Task UpdateEntry(long id, Action<SyncLog> update, CancellationToken cancellationToken)
    {
        var entry = await ctx.SyncLogs.FindAsync([id], cancellationToken);
        if (entry is null) return;

        update(entry);
        await ctx.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> GetCurrentSeasonId(CancellationToken cancellationToken)
    {
        var seasonIdValue = await ctx.AppConfigs
            .AsNoTracking()
            .Where(c => c.Key == "current_season")
            .Select(c => c.Value)
            .SingleOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(seasonIdValue))
            throw new InvalidOperationException("The 'current_season' app config value was not found.");

        if (!Guid.TryParse(seasonIdValue, out var seasonId))
            throw new InvalidOperationException("The 'current_season' app config value is not a valid GUID.");

        return seasonId;
    }
    
    
}