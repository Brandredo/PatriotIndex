using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Jobs;

namespace PatriotIndex.Domain.Repository;

public class SeasonsRepository(ILogger<SeasonsRepository> logger, PatriotIndexDbContext ctx)
{
    public async Task<List<Season>> GetAllSeasonsAsync(CancellationToken cancellationToken = default)
    {
        return await ctx.Seasons.AsNoTracking().ToListAsync(cancellationToken);
    }
    
    
    public async Task<List<SeasonInput>> GetAllSeasonCodesAndYearAsync(CancellationToken cancellationToken = default)
    {
        return await ctx.Seasons.AsNoTracking().Select(s => new SeasonInput
        {
            SeasonYear = s.Year,
            SeasonType = s.Code
        }).ToListAsync(cancellationToken);
    }
    
    public async Task<List<SeasonInput>> GetRegSeasonCodesAndYearAsync(CancellationToken cancellationToken = default)
    {
        return await ctx.Seasons.AsNoTracking().Where(s => s.Code == "REG").Select(s => new SeasonInput
        {
            SeasonYear = s.Year,
            SeasonType = s.Code
        }).ToListAsync(cancellationToken);
    }
    
    public async Task<List<SeasonInput>> GetPstSeasonCodesAndYearAsync(CancellationToken cancellationToken = default)
    {
        return await ctx.Seasons.AsNoTracking().Where(s => s.Code == "PST").Select(s => new SeasonInput
        {
            SeasonYear = s.Year,
            SeasonType = s.Code
        }).ToListAsync(cancellationToken);
    }
    
    public async Task<List<SeasonInput>> GetRegSeasonCodesAndYearAsync(int year, CancellationToken cancellationToken = default)
    {
        return await ctx.Seasons.AsNoTracking().Where(s => s.Code == "REG" && s.Year == year).Select(s => new SeasonInput
        {
            SeasonYear = s.Year,
            SeasonType = s.Code
        }).ToListAsync(cancellationToken);
    }
    
    public async Task<List<SeasonInput>> GetPstSeasonCodesAndYearAsync(int year, CancellationToken cancellationToken = default)
    {
        return await ctx.Seasons.AsNoTracking().Where(s => s.Code == "PST" && s.Year == year).Select(s => new SeasonInput
        {
            SeasonYear = s.Year,
            SeasonType = s.Code
        }).ToListAsync(cancellationToken);
    }
    
    public async Task<List<SeasonInput>> GetPstByYearUpAsync(int year, CancellationToken cancellationToken = default)
    {
        return await ctx.Seasons.AsNoTracking().Where(s => s.Code == "PST" && s.Year >= year).Select(s => new SeasonInput
        {
            SeasonYear = s.Year,
            SeasonType = s.Code
        }).ToListAsync(cancellationToken);
    }
    
    
    
}
