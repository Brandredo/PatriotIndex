using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Enums;
using PatriotIndex.Domain.Interfaces;
using StackExchange.Redis;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class PlayerController(IConnectionMultiplexer connectionMux, IPlayerRepository players) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var player = await players.GetByIdAsync(id);
        return player is null ? NotFound() : Ok(player);
    }

    [HttpGet("{id:guid}/gamelog")]
    public async Task<IActionResult> GetGameLog(
        Guid id,
        [FromQuery] int? season,
        [FromQuery] string? seasonType)
        => Ok(await players.GetGameLogAsync(id, season, seasonType));

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? search,
        [FromQuery] Guid? teamId,
        [FromQuery] PlayerPosition? position,
        [FromQuery] string? status)
        => Ok(await players.SearchAsync(search, teamId, position, status));
}
