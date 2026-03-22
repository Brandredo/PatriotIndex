using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class StatsRepository(PatriotIndexDbContext_OLD ctx, ILogger<StatsRepository> logger)
{

    public async Task SaveAsync(IReadOnlyList<TeamSeasonStats> teamStats, IReadOnlyList<PlayerSeasonStats> playerStats,
        CancellationToken ct = default)
    {
        
    }
    
    
    
    /// <summary>
    /// Upserts team season stats and all associated player season stats in a single transaction.
    /// Uses delete + re-insert because EF Core serialises the JSONB blocks automatically.
    /// </summary>
    public async Task SaveAsync(
        TeamSeasonStats teamStats,
        IEnumerable<Player> players,
        IEnumerable<PlayerSeasonStats> playerStats,
        CancellationToken ct = default)
    {
        var playerList = players.ToList();
        var statsList  = playerStats.ToList();

        var strategy = ctx.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await ctx.Database.BeginTransactionAsync(ct);
            try
            {
                await UpsertTeamSeasonStatsAsync(teamStats, ct);
                await UpsertPlayersAsync(playerList, ct);
                await UpsertPlayerSeasonStatsAsync(statsList, ct);
                await tx.CommitAsync(ct);
                logger.LogInformation(
                    "Saved season stats for team {TeamId} {Year} {Type} with {Count} player records.",
                    teamStats.TeamId, teamStats.SeasonYear, teamStats.SeasonType, statsList.Count);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                logger.LogError(ex, "Failed to save season stats for team {TeamId}.", teamStats.TeamId);
                throw;
            }
        });
    }

    // ─────────────────────────────────────────────────────────────────────

    private async Task UpsertTeamSeasonStatsAsync(TeamSeasonStats stats, CancellationToken ct)
    {
        // Delete any existing row for this team + season to allow clean re-insert.
        // EF Core handles JSONB serialisation on SaveChanges, so we avoid raw SQL for the insert.
        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM team_season_stats WHERE team_id = {0} AND season_year = {1} AND season_type = {2}",
            new object[] { stats.TeamId, stats.SeasonYear, stats.SeasonType },
            ct);

        ctx.TeamSeasonStats.Add(stats);
        await ctx.SaveChangesAsync(ct);
        ctx.ChangeTracker.Clear();

        logger.LogDebug(
            "Upserted TeamSeasonStats for {TeamId} {Year}/{Type}.",
            stats.TeamId, stats.SeasonYear, stats.SeasonType);
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

        logger.LogDebug("Upserted {Count} thin player records.", players.Count);
    }

    private async Task UpsertPlayerSeasonStatsAsync(List<PlayerSeasonStats> players, CancellationToken ct)
    {
        if (players.Count == 0) return;

        // Collect the player IDs present in this payload and delete their existing season rows.
        var playerIds = players.Select(p => p.PlayerId).Distinct().ToArray();
        var seasonYear = players[0].SeasonYear;
        var seasonType = players[0].SeasonType;

        await ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM player_season_stats WHERE player_id = ANY({0}) AND season_year = {1} AND season_type = {2}",
            new object[] { playerIds, seasonYear, seasonType },
            ct);

        await ctx.PlayerSeasonStats.AddRangeAsync(players, ct);
        await ctx.SaveChangesAsync(ct);
        ctx.ChangeTracker.Clear();

        logger.LogDebug(
            "Upserted {Count} PlayerSeasonStats for season {Year}/{Type}.",
            players.Count, seasonYear, seasonType);
    }
}
