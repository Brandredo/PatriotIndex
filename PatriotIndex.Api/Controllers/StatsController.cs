using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Interfaces;
using PatriotIndex.Domain.Queries;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController(
    IPlayerRepository players,
    ITeamRepository teams,
    ICacheService cache,
    StatisticsQueryService statisticsQueryService) : ControllerBase
{
    [HttpGet("players")]
    public async Task<IActionResult> GetPlayersStats(
        [FromQuery] string? position,
        [FromQuery] int season = 2025,
        [FromQuery] string seasonType = "REG",
        [FromQuery] string? cursor = null,
        [FromQuery] int limit = 15,
        CancellationToken ct = default)
    {
        var cacheKey = $"stats:players:{position ?? "all"}:{season}:{seasonType}:{cursor ?? "start"}:{limit}";
        var results = await cache.GetOrSetAsync(
            cacheKey,
            () => statisticsQueryService.GetPlayerSeasonStats(position, season, seasonType, cursor, limit, ct),
            season < DateTime.UtcNow.Year ? TimeSpan.FromHours(24) : TimeSpan.FromMinutes(30),
            ct);

        return Ok(new
        {
            Players = results.Items,
            results.NextCursor,
            Total = results.TotalCount
        });
    }

    [HttpGet("teams")]
    public async Task<IActionResult> GetTeamsStats(
        [FromQuery] int season = 2025,
        [FromQuery] string seasonType = "REG",
        [FromQuery] string? cursor = null,
        [FromQuery] int limit = 10,
        CancellationToken ct = default)
    {
        var cacheKey = $"stats:teams:{season}:{seasonType}:{cursor ?? "start"}:{limit}";
        var results = await cache.GetOrSetAsync(
            cacheKey,
            () => statisticsQueryService.GetTeamSeasonStats(season, seasonType, cursor, limit, ct),
            season < DateTime.UtcNow.Year ? TimeSpan.FromHours(24) : TimeSpan.FromMinutes(30),
            ct);

        return Ok(new
        {
            Teams = results.Items,
            results.NextCursor,
            Total = results.TotalCount
        });
    }
}