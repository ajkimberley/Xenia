using MediatR;

using Microsoft.AspNetCore.Mvc;

using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
    public async Task<IActionResult> AddHotel(HotelDto dto)
    {
        // Create hotel command
        // Execute command
        var cmd = new CreateHotelCommand(dto.Name);
        await _mediator.Send(cmd);

        // Either poll or redirect to get method
        return Created("foo", null);
    }
}
