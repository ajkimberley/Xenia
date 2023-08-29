﻿using MediatR;

using Microsoft.AspNetCore.Mvc;

using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;
using ScreenMedia.Xenia.WebApi.Queries;

namespace ScreenMedia.Xenia.WebApi.Controllers;
[Route("api/hotels/{hotelId:Guid}/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator) => _mediator = mediator;

    [HttpGet(Name = nameof(GetRooms))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoomDto>))]
    public async Task<IActionResult> GetRooms([FromRoute] Guid hotelId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        try
        {
            var qry = new GetRoomsQuery(hotelId, from, to);
            var dtos = await _mediator.Send(qry);

            return Ok(dtos);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
