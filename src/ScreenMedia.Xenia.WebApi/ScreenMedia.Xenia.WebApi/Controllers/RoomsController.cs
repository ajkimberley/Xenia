using Microsoft.AspNetCore.Mvc;

using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Controllers;
[Route("api/hotels/{hotelId}/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    [HttpGet(Name = nameof(GetRooms))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoomDto>))]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<IActionResult> GetRooms()
    {
        var dtos = new List<RoomDto> { new RoomDto() };
        return Ok(dtos);
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
