using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Interfaces;
using StackExchange.Redis;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class ConferenceController(IConnectionMultiplexer connectionMux, IConferenceRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await repo.GetAllWithDivisionsAsync());
}
