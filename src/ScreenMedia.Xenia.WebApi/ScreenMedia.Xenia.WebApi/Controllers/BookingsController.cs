using MediatR;

using Microsoft.AspNetCore.Mvc;

using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator) => _mediator = mediator;

    [HttpPost(Name = nameof(CreateBooking))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookingResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBooking(BookingDto dto)
    {
        if (dto is null) return BadRequest("Request body was invalid.");
        var cmd = new BookRoomCommand(dto.HotelId, dto.RoomId, "Joe Bloggs", "j.bloggs@example.com", dto.From, dto.To);
        var resposne = await _mediator.Send(cmd);

        // TODO: Get Booking Resource
        return Created("Foo", resposne);
    }
}
