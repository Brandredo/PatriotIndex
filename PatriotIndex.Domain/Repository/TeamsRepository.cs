using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class TeamsRepository(IDbContextFactory<PatriotIndexDbContext> contextFactory, ILogger<TeamsRepository> logger)
{

    public async Task<IEnumerable<Team>> GetTeamsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all teams");
        await using var ctx = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await ctx.Teams.ToListAsync(cancellationToken);
    }
    
    public async Task<Team?> GetTeamByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Getting team with id: {id}");
        await using var ctx = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await ctx.Teams.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Guid>> GetTeamIdsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all team ids");
        await using var ctx = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await ctx.Teams.Select(t => t.Id).ToListAsync(cancellationToken);
    }
    
    // update a team's profile using Postgres ON CONFLICT DO UPDATE
    
    
    // delete a team based on the team's id
    
    
}