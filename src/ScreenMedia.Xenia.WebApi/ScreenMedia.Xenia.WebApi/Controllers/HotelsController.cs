using MediatR;


using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;
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

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var hotelId)) return BadRequest("Invalid hotel Id.");

        try
        {
            var qry = new GetHotelQuery(hotelId);
            var dto = await _mediator.Send(qry);
            return Ok(dto);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
    public async Task<IActionResult> AddHotel(HotelDto dto)
    {
        var cmd = new CreateHotelCommand(dto.Name);
        var createdHotel = await _mediator.Send(cmd);

        var hotelLinks = new List<LinkDto>();
        //{
        //    new LinkDto(Url.Link(nameof(GetHotel), new { id = createdHotel.Id }), "self", "GET"),
        //    new LinkDto(Url.Link(nameof(UpdateHotel), new { id = createdHotel.Id }), "update_hotel", "PUT"),
        //    new LinkDto(Url.Link(nameof(DeleteHotel), new { id = createdHotel.Id }), "delete_hotel", "DELETE")
        //};

        var hotelCreatedResponse = new HotelCreatedResponse(createdHotel, hotelLinks);

        return Created($"localhost:7072/api/hotels/{createdHotel.Id}", hotelCreatedResponse);
    }
}
