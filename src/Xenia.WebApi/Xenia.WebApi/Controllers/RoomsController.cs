using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Xenia.Application.Dtos;
using Xenia.Application.Queries;

namespace Xenia.WebApi.Controllers;

[Route("api/hotels/{hotelId:Guid}/[controller]")]
[ApiController]
public class RoomsController(ISender mediator) : ControllerBase
{
    [HttpGet(Name = nameof(GetRooms))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoomDto>))]
    public async Task<IActionResult> GetRooms([FromRoute] Guid hotelId, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var qry = new GetAvailableRoomsQuery(hotelId, from, to);
        var result = await mediator.Send(qry);
        return result.MatchFirst<IActionResult>(
            value => new OkObjectResult(value),
            err => err.Type switch
            {
                ErrorType.Unexpected when err.Code == "RestError.ResourceNotFound" => new NotFoundObjectResult(err),
                ErrorType.Validation => new BadRequestObjectResult(err.Description),
                _ => new ObjectResult(new { error = new ArgumentOutOfRangeException(nameof(err.Type), err.Type, err.Description) }) { StatusCode = 500 }
            });
    }
}