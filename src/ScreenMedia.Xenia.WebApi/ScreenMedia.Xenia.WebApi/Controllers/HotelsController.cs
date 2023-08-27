using Microsoft.AspNetCore.Mvc;

using ScreenMedia.Xenia.HotelManagement.Domain;
using ScreenMedia.Xenia.HotelManagement.Persistence;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly HotelManagementContext _hotelManagementContext;

    public HotelsController(HotelManagementContext hotelManagementContext)
        => _hotelManagementContext = hotelManagementContext;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
    public async Task<IActionResult> AddHotel(HotelDto dto)
    {
        // Create hotel command
        // Execute command
        var hotel = Hotel.Create(dto.Name);

        _ = _hotelManagementContext.Add(hotel);
        _ = await _hotelManagementContext.SaveChangesAsync();

        // Either poll or redirect to get method
        return Created("foo", null);
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
