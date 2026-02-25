using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion.Persistence;

public class GamePbpSaver(ILogger<GamePbpSaver> logger)
{
    public async Task SaveAsync(Game game, IDbContextFactory<PatriotIndexDbContext> dbFactory, CancellationToken ct)
    {
        await using var ctx = await dbFactory.CreateDbContextAsync(ct);
        bool exists = await ctx.Games.AnyAsync(g => g.Id == game.Id, ct);
        if (exists)
        {
            logger.LogWarning("Game {GameId} already exists, skipping.", game.Id);
            return;
        }
        ctx.Games.Add(game);
        await ctx.SaveChangesAsync(ct);
    }
    
    // method to clear a game
}
