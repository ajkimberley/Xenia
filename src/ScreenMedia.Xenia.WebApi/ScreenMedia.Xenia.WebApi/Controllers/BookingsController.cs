using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using ScreenMedia.Xenia.Bookings.Domain.Errors;
using ScreenMedia.Xenia.WebApi.Commands;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Errors;
using ScreenMedia.Xenia.WebApi.Exceptions;
using ScreenMedia.Xenia.WebApi.Queries;

namespace ScreenMedia.Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator) => _mediator = mediator;

    [HttpGet(Name = nameof(GetBookings))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BookingDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookings([FromQuery] string? bookingReference)
    {
        var qry = new GetBookingsQuery(bookingReference);
        var dtos = await _mediator.Send(qry);

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
        var cmd = new BookRoomCommand(dto.HotelId, dto.RoomType, dto.BookerName, dto.BookerEmail, dto.From, dto.To);
        var result = await _mediator.Send(cmd);

        return result.Match<IActionResult>(
            createdBooking =>
            {
                // TODO: Validate Host header
                var selfLink = Url.Link(nameof(GetBooking), new { id = createdBooking.Id.ToString() });
                var bookingLinks = new List<LinkDto> { new(selfLink!, "self", "GET") };

                var bookingResponse = new BookingResponseDto(createdBooking, bookingLinks);
                return Created("Foo", bookingResponse);
            },
            ex =>
            {
                return ex switch
                {
                    MaximumRetryError => new ConflictObjectResult(
                        "Maximum number of retries were attempted in booking."),
                    NoVacanciesError noVacanciesError => new ConflictObjectResult(noVacanciesError.Message),
                    ResourceNotFoundError resourceNotFoundError => new NotFoundObjectResult(resourceNotFoundError),
                    _ => new ObjectResult($"An unexpected error has occured. Inner error: {ex.Message}")
                    {
                        StatusCode = 500
                    }
                };
            });
    }
}