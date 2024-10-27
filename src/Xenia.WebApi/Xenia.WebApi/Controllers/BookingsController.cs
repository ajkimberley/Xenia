﻿using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Xenia.Application.Commands;
using Xenia.Application.Queries;
using Xenia.Common.Dtos;

namespace Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(ISender mediator) : ControllerBase
{
    [HttpGet(Name = nameof(GetBookings))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BookingDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookings([FromQuery] string? bookingReference)
    {
        var qry = new GetBookingsQuery(bookingReference);
        var dtos = await mediator.Send(qry);

        return dtos.IsNullOrEmpty()
            ? bookingReference != null ? NotFound() : NoContent()
            : Ok(dtos);
    }

    [HttpGet("{id:Guid}", Name = nameof(GetBooking))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HotelDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        var qry = new GetBookingQuery(id);

        return (await mediator.Send(qry))
            .Match<IActionResult>(Ok, NotFound);
    }

    [HttpPost(Name = nameof(CreateBooking))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookingResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBooking(BookingDto dto)
    {
        var cmd = new BookRoomCommand(dto.HotelId, dto.RoomType, dto.BookerName, dto.BookerEmail, dto.From, dto.To);
        var result = await mediator.Send(cmd);

        return result.MatchFirst<IActionResult>(
            createdBooking =>
            {
                // TODO: Validate Host header
                var selfLink = Url.Link(nameof(GetBooking), new { id = createdBooking.Id.ToString() });
                var bookingLinks = new List<LinkDto> { new(selfLink!, "self", "GET") };

                var bookingResponse = new BookingResponseDto(createdBooking, bookingLinks);
                return Created("Foo", bookingResponse);
            },
            error =>
            {
                return error.Type switch
                {
                    ErrorType.Conflict => new ConflictResult(),
                    ErrorType.NotFound => new NotFoundObjectResult(error),
                    _ => new ObjectResult($"An unexpected error has occured. Inner error: {error}") { StatusCode = 500 }
                };
            });
    }
}