﻿using MediatR;

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

    [HttpGet("{id}", Name = nameof(GetHotel))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel(string id)
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

    [HttpPost(Name = nameof(CreateHotel))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HotelResponse))]
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

        var hotelCreatedResponse = new HotelResponse(createdHotel, hotelLinks);

        return Created(selfLink!, hotelCreatedResponse);
    }
}