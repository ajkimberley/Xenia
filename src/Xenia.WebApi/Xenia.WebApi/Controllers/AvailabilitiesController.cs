using MediatR;

using Microsoft.AspNetCore.Mvc;

using Modules.HotelAdmin.Application.SetAvailability;

using Xenia.WebApi.RequestResponse;

namespace Xenia.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvailabilitiesController(ISender mediator) : ControllerBase
{
    [HttpPut(Name = nameof(SetAvailability))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AvailabilitiesResponse))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AvailabilitiesResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetAvailability(AvailabilitiesRequest request)
    {
        var cmd = new SetAvailabilityCommand(request.HotelId, request.RoomType, request.Date, request.AvailableRooms);
        var result = await mediator.Send(cmd);

        return result.Match<IActionResult>(
            _ => CreatedAtAction(
                nameof(SetAvailability), 
                new { request.HotelId, request.RoomType, request.Date }, 
                new AvailabilitiesResponse()
            ),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        );
    }
}