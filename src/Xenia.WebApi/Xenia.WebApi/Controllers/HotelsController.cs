using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Xenia.WebApi.Commands;
using Xenia.WebApi.Dtos;
using Xenia.WebApi.Exceptions;
using Xenia.WebApi.Queries;

namespace Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelsController(IMediator mediator) => _mediator = mediator;

    [HttpGet(Name = nameof(GetHotels))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<HotelDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotels([FromQuery] string? name)
    {
        var qry = new GetHotelsQuery(name);
        var dtos = await _mediator.Send(qry);

        return dtos.IsNullOrEmpty()
            ? name != null ? NotFound() : NoContent() : Ok(dtos);
    }

    [HttpGet("{id:guid}", Name = nameof(GetHotel))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel(Guid id)
    {
        try
        {
            var qry = new GetHotelQuery(id);
            var dto = await _mediator.Send(qry);
            return Ok(dto);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost(Name = nameof(CreateHotel))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HotelResponseDto))]
    public async Task<IActionResult> CreateHotel(HotelDto dto)
    {
        var cmd = new CreateHotelCommand(dto.Name);
        var createdHotel = await _mediator.Send(cmd);

        // TODO: Validate Host header
        var selfLink = Url.Link(nameof(GetHotel), new { id = createdHotel.Id.ToString() });
        var hotelLinks = new List<LinkDto>
        {
            new LinkDto(selfLink!, "self", "GET")
        };

        var hotelCreatedResponse = new HotelResponseDto(createdHotel, hotelLinks);
        return Created(selfLink!, hotelCreatedResponse);
    }
}
