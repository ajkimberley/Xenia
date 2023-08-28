using System.Net;

using ScreenMedia.Xenia.HotelManagement.Domain.Entities;
using ScreenMedia.Xenia.HotelManagement.Persistence;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.IntegrationTests;

[Collection("WebApi Collection")]
public sealed class RoomControllerTests
{
    private readonly XeniaWebApplicationFactory<Program> _applicationFactory;

    public RoomControllerTests(XeniaWebApplicationFactory<Program> applicationFactory) =>
        _applicationFactory = applicationFactory;

    [Fact]
    public async Task GetRoomsReturns200AndRoomsResourceWhenHotelHasRooms()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<HotelManagementContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(HotelManagementContext)}");

        var hotel = Hotel.Create("Foo");
        _ = context.Add(hotel);
        _ = context.SaveChanges();

        var expected = new List<RoomDto> { new RoomDto() };

        var response = await client.GetAsync($"api/hotels/{hotel.Id}/Rooms");
        var actual = await response.Content.ReadFromJsonAsync<List<RoomDto>>();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected, actual);
        });
    }
}
