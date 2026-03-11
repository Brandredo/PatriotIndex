using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class TeamController(ITeamRepository teams) : ControllerBase
{
    [HttpGet("hello")]
    public string Hello() => "Hello World!";
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await teams.GetTeamsAndPlayers());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var team = await teams.GetByIdAsync(id);
        return team is null ? NotFound() : Ok(team);
    }

    [HttpGet("{id:guid}/roster")]
    public async Task<IActionResult> GetRoster(Guid id)
        => Ok(await teams.GetRosterAsync(id));

    [HttpGet("{id:guid}/stats")]
    public async Task<IActionResult> GetStats(
        Guid id,
        [FromQuery] int season = 2024,
        [FromQuery] string seasonType = "REG")
    {
        var stats = await teams.GetSeasonStatsAsync(id, season, seasonType);
        return stats is null ? NotFound() : Ok(stats);
    }
}
