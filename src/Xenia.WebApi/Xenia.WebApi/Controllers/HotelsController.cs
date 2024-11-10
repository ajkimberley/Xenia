using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Xenia.Application.Commands;
using Xenia.Application.Dtos;
using Xenia.Application.Queries;
using Xenia.WebApi.RequestsResponses;

namespace Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(ISender mediator) : ControllerBase
{
    [HttpGet(Name = nameof(GetHotels))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<HotelDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotels([FromQuery] GetHotelsRequest? query)
    {
        var name = query?.Name;
        
        var qry = new GetHotelsQuery(name);
        var dtos = await mediator.Send(qry);
        
        if (name != null) return dtos.IsNullOrEmpty() ? NotFound() : Ok(dtos);
        return Ok(dtos);
    }

    [HttpGet("{id:guid}", Name = nameof(GetHotel))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel(Guid id)
    {
            var qry = new GetHotelQuery(id);

            return (await mediator.Send(qry)).Match<IActionResult>(Ok, NotFound);
    }

    [HttpPost(Name = nameof(CreateHotel))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HotelResponseDto))]
    public async Task<IActionResult> CreateHotel(HotelDto dto)
    {
        var cmd = new CreateHotelCommand(dto.Name);
        var createdHotel = await mediator.Send(cmd);

        // TODO: Validate Host header
        var selfLink = Url.Link(nameof(GetHotel), new { id = createdHotel.Id.ToString() });
        var hotelLinks = new List<LinkDto>
        {
            new(selfLink!, "self", "GET")
        };

        var hotelCreatedResponse = new HotelResponseDto(createdHotel, hotelLinks);
        return Created(selfLink!, hotelCreatedResponse);
    }
}
