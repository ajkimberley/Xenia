using MediatR;

using Microsoft.AspNetCore.Mvc;

using Xenia.Application.Commands;
using Xenia.Application.Dtos;

namespace Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeedController(IMediator mediator) => _mediator = mediator;

    [HttpPost(Name = nameof(Seed))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelResponseDto))]
    public async Task<IActionResult> Seed()
    {
        var cmd = new SeedDataCommand();
        var seededData = await _mediator.Send(cmd);
        return Ok(seededData);
    }

    [HttpDelete(Name = nameof(Unseed))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelResponseDto))]
    public async Task<IActionResult> Unseed()
    {
        var cmd = new UnseedDataCommand();
        await _mediator.Send(cmd);
        return Ok();
    }
}
