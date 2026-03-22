using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Interfaces;
using StackExchange.Redis;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(IConnectionMultiplexer connectionMux, ILeaderboardRepository leaderboard)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string category = "PASS_YDS",
        [FromQuery] int season = 2024,
        [FromQuery] string seasonType = "REG",
        [FromQuery] PlayerPosition? position = null,
        [FromQuery] int limit = 25)
    {
        return Ok(await leaderboard.GetLeaderboardAsync(category, season, seasonType, position, limit));
    }
}