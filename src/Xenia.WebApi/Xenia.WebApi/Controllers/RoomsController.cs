using MediatR;

using Microsoft.AspNetCore.Mvc;

using Xenia.WebApi.Dtos;
using Xenia.WebApi.Exceptions;
using Xenia.WebApi.Queries;

namespace Xenia.WebApi.Controllers;

[Route("api/hotels/{hotelId:Guid}/[controller]")]
[ApiController]
public class RoomsController(ISender mediator) : ControllerBase
{
    [HttpGet(Name = nameof(GetRooms))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoomDto>))]
    public async Task<IActionResult> GetRooms([FromRoute] Guid hotelId, [FromQuery] DateTime? from, DateTime? to)
    {
        try
        {
            var qry = new GetAvailableRoomsQuery(hotelId, from, to);
            var result = await mediator.Send(qry);
            return result.Match<IActionResult>(Ok, BadRequest);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
