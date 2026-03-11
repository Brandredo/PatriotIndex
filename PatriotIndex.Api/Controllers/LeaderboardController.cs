using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(ILeaderboardRepository leaderboard) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string category = "PASS_YDS",
        [FromQuery] int season = 2024,
        [FromQuery] string seasonType = "REG",
        [FromQuery] PlayerPosition? position = null,
        [FromQuery] int limit = 25)
        => Ok(await leaderboard.GetLeaderboardAsync(category, season, seasonType, position, limit));
}
