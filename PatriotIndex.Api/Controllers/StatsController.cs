using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController(IPlayerRepository players, ITeamRepository teams) : ControllerBase
{
    [HttpGet("players")]
    public async Task<IActionResult> GetPlayersStats(
        [FromQuery] string? position,
        [FromQuery] int season = 2024,
        [FromQuery] string seasonType = "REG",
        [FromQuery] string? cursor = null,
        [FromQuery] int limit = 200,
        CancellationToken ct = default)
        => Ok(await players.GetAllPlayersStatsAsync(position, season, seasonType, cursor, limit, ct));

    [HttpGet("teams")]
    public async Task<IActionResult> GetTeamsStats(
        [FromQuery] int season = 2024,
        [FromQuery] string seasonType = "REG",
        [FromQuery] string? cursor = null,
        [FromQuery] int limit = 32,
        CancellationToken ct = default)
        => Ok(await teams.GetAllTeamsStatsAsync(season, seasonType, cursor, limit, ct));
}
