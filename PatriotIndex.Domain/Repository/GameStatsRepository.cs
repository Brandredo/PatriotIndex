using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class GameStatsRepository(PatriotIndexDbContext_OLD ctx, ILogger<GameStatsRepository> logger)
{
    public async Task SaveAsync(
        List<TeamGameStats> teamStats,
        List<PlayerGameStats> playerStats,
        List<Player> players,
        CancellationToken ct = default)
    {
        if (teamStats.Count == 0 && playerStats.Count == 0) return;

        var gameId   = teamStats.FirstOrDefault()?.GameId ?? playerStats.FirstOrDefault()?.GameId ?? Guid.Empty;
        var strategy = ctx.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await ctx.Database.BeginTransactionAsync(ct);
            try
            {
                await UpsertPlayersAsync(players, ct);

                foreach (var ts in teamStats)
                    await UpsertTeamGameStatsAsync(ts, ct);

                await UpsertPlayerGameStatsAsync(gameId, playerStats, ct);

                await tx.CommitAsync(ct);
                logger.LogInformation(
                    "Saved game stats for game {GameId}: {TeamCount} team rows, {PlayerCount} player rows, {PlayerStubCount} player stubs.",
                    gameId, teamStats.Count, playerStats.Count, players.Count);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                logger.LogError(ex, "Failed to save game stats for game {GameId}.", gameId);
                throw;
            }
        });
    }

    private async Task UpsertPlayersAsync(List<Player> players, CancellationToken ct)
    {
        if (players.Count == 0) return;

        foreach (var p in players)
        {
            await ctx.Database.ExecuteSqlInterpolatedAsync(
                $"""
                INSERT INTO players (id, team_id, first_name, last_name, name, jersey, position, sr_id)
                VALUES ({p.Id}, {p.TeamId}, {p.FirstName}, {p.LastName}, {p.Name}, {p.Jersey}, {(int?)p.Position}, {p.SrId})
                ON CONFLICT (id) DO NOTHING
                """,
                ct);
        }

        logger.LogDebug("Upserted {Count} thin player stubs.", players.Count);
    }

    private async Task UpsertTeamGameStatsAsync(TeamGameStats stats, CancellationToken ct)
    {
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM team_game_stats WHERE game_id = {0} AND team_id = {1}",
            new object[] { stats.GameId, stats.TeamId },
            ct);

        ctx.TeamGameStats.Add(stats);
        await ctx.SaveChangesAsync(ct);
        ctx.ChangeTracker.Clear();
    }

    private async Task UpsertPlayerGameStatsAsync(
        Guid gameId, List<PlayerGameStats> players, CancellationToken ct)
    {
        if (players.Count == 0) return;

        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM player_game_stats WHERE game_id = {0}",
            new object[] { gameId },
            ct);

        await ctx.PlayerGameStats.AddRangeAsync(players, ct);
        await ctx.SaveChangesAsync(ct);
        ctx.ChangeTracker.Clear();
    }
}
