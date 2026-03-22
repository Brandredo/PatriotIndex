using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Interfaces;
using StackExchange.Redis;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class GameController(IConnectionMultiplexer connectionMux, IGameRepository games) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? season,
        [FromQuery] string? seasonType,
        [FromQuery] int? week,
        [FromQuery] Guid? teamId)
    {
        return Ok(await games.GetGamesAsync(season, seasonType, week, teamId));
    }

    [HttpGet("{id:guid}/pbp")]
    public async Task<IActionResult> GetPbp(Guid id)
    {
        var pbp = await games.GetGamePbpAsync(id);
        return pbp is null ? NotFound() : Ok(pbp);
    }
}