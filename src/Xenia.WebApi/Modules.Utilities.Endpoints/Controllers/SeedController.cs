using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Modules.HotelAdmin.Application;
using Modules.Utilities.Application.Seeding;

namespace Modules.Utilities.Endpoints.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController(IMediator mediator) : ControllerBase
{
    [HttpPost(Name = nameof(Seed))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelResponseDto))]
    public async Task<IActionResult> Seed()
    {
        var cmd = new SeedDataCommand();
        var seededData = await mediator.Send(cmd);
        return Ok(seededData);
    }

    [HttpDelete(Name = nameof(Unseed))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelResponseDto))]
    public async Task<IActionResult> Unseed()
    {
        var cmd = new UnseedDataCommand();
        await mediator.Send(cmd);
        return Ok();
    }
}
