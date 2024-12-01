using MediatR;

using Microsoft.AspNetCore.Mvc;

using Xenia.Application.HotelManagement;
using Xenia.Application.Seeding;

namespace Xenia.WebApi.Controllers;

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
