using Microsoft.AspNetCore.Mvc;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class ConferenceController(IConferenceRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await repo.GetAllWithDivisionsAsync());
}
