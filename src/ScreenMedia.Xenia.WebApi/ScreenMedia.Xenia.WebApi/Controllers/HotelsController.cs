using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Queries;

namespace ScreenMedia.Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetHotels()
    {
        var qry = new GetHotelsQuery();
        var dtos = await _mediator.Send(qry);

        return dtos.IsNullOrEmpty() ? NoContent() : Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
    public async Task<IActionResult> AddHotel(HotelDto dto)
    {
        var cmd = new CreateHotelCommand(dto.Name);
        var id = await _mediator.Send(cmd);

        return Created($"localhost:7072/api/hotels/{id}", id);
    }
}
