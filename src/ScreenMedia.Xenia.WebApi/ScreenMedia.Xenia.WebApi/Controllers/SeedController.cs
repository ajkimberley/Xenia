using MediatR;

using Microsoft.AspNetCore.Mvc;

using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly IMediator _mediator;

    public SeedController(IMediator mediator) => _mediator = mediator;

    [HttpPost(Name = nameof(Seed))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HotelResponseDto))]
    public async Task<IActionResult> Seed()
    {
        var cmd = new SeedDataCommand();
        var seededData = await _mediator.Send(cmd);
        return Ok(seededData);
    }
}
