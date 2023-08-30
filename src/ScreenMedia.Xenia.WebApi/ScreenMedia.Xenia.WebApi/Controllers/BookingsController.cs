﻿using MediatR;

using Microsoft.AspNetCore.Mvc;

using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;
using ScreenMedia.Xenia.WebApi.Queries;

namespace ScreenMedia.Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:Guid}", Name = nameof(GetBooking))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        try
        {
            var qry = new GetBookingQuery(id);
            var dto = await _mediator.Send(qry);
            return Ok(dto);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost(Name = nameof(CreateBooking))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookingResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBooking(BookingDto dto)
    {
        var cmd = new BookRoomCommand(dto.HotelId, dto.RoomType, "Joe Bloggs", "j.bloggs@example.com", dto.From, dto.To);
        var createdBooking = await _mediator.Send(cmd);

        // TODO: Validate Host header
        var selfLink = Url.Link(nameof(GetBooking), new { id = createdBooking.Id.ToString() });
        var bookingLinks = new List<LinkDto>
        {
            new LinkDto(selfLink!, "self", "GET")
        };

        var bookingResponse = new BookingResponseDto(createdBooking, bookingLinks);
        return Created("Foo", bookingResponse);
    }
}
