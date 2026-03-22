using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class ConferenceQueryRepository(PatriotIndexDbContext_OLD db, ICacheService cache) : IConferenceRepository
{
    public async Task<IReadOnlyList<ConferenceWithDivisionsDto>> GetAllWithDivisionsAsync()
    {
        var conferences = await db.Conferences
            .AsNoTracking()
            .Include(c => c.Divisions)
                .ThenInclude(d => d.Teams.Where(t => t.IsActive))
                    .ThenInclude(t => t.Colors)
            .OrderBy(c => c.Alias)
            .ToListAsync();

        var result = conferences.Select(c => new ConferenceWithDivisionsDto(
            c.Id, c.Name, c.Alias,
            c.Divisions
                .OrderBy(d => d.Name)
                .Select(d => new DivisionWithTeamsDto(
                    d.Id, d.Name, d.Alias,
                    d.Teams
                        .OrderBy(t => t.Market)
                        .Select(t => new TeamSummaryDto(
                            t.Id, t.Name, t.Market, t.Alias,
                            t.Colors,
                            new DivisionSummaryDto(d.Id, d.Name, d.Alias,
                                new ConferenceSummaryDto(c.Id, c.Name, c.Alias))))
                        .ToList()))
                .ToList()))
            .ToList();
        
        return result;
    }

    public async Task<IReadOnlyList<ConferenceWithDivisionsDto>> GetAllConferencesAsync()
    {
        var cacheKey = $"league:hierarchy";
        var full = await cache.GetOrSetAsync(
            cacheKey,
            GetAllWithDivisionsAsync, TimeSpan.FromDays(30));

        if (full is null)
        {
            throw new Exception("Failed to get conference hierarchy from cache");
        }
        
        return full;
    }
}
