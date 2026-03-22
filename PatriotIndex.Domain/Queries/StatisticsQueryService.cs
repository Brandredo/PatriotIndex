using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Frontend;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class StatisticsQueryService(IPlayerRepository playerRepository, IPlayerStatsService playerStatsService, ITeamRepository teamRepository, ITeamStatsService teamStatsService)
{
    
    // method gets the player season stats
    public async Task<PagedResultDto<PlayerStatsSummaryDto>> GetPlayerSeasonStats(string? positionGroup, int season, string seasonType,
        string? cursor, int limit, CancellationToken ct = default)
    {
        var results = await playerRepository.GetAllPlayersStatsAsync(positionGroup, season, seasonType, cursor, limit, ct);

        var calculatedStats = results.Items.Select(p => new PlayerStatsSummaryDto
        (
            p.PlayerId,
            p.Player?.Name,
            p.Player?.Position.ToString(),
            p.TeamId,
            p.Player?.Team?.Alias,
            p.Player?.Team?.Market,
            p.GamesPlayed,
            p.GamesStarted,
            playerStatsService.Calculate(p.Stats)
        )).ToArray();

        return new PagedResultDto<PlayerStatsSummaryDto>(calculatedStats, results.NextCursor, results.TotalCount);
    }

    public async Task<PagedResultDto<TeamStatsSummaryDto>> GetTeamSeasonStats(int season, string seasonType, string? cursor, int limit, CancellationToken ct = default)
    {
        var results = await teamRepository.GetAllTeamsStatsAsync(season, seasonType, cursor, limit, ct);

        var calculatedStats = results.Items.Select(t => new TeamStatsSummaryDto(
            t.TeamId,
            t.Team?.Name,
            t.Team?.Market,
            t.Team?.Alias,
            t.Team?.Colors,
            t.GamesPlayed,
            teamStatsService.Calculate(t.Record)
        )).ToArray();

        return new PagedResultDto<TeamStatsSummaryDto>(calculatedStats, results.NextCursor, results.TotalCount);
    }
    
    
}