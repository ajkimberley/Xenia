using System.Net;

using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Bookings.Persistence;
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
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Foo");
        _ = context.Add(hotel);
        _ = context.SaveChanges();

        var expected = new List<RoomDto>
        {
            new RoomDto(hotel.Id, RoomType.Single, 1),
            new RoomDto(hotel.Id, RoomType.Double, 2),
            new RoomDto(hotel.Id, RoomType.Double, 2),
            new RoomDto(hotel.Id, RoomType.Deluxe, 2),
            new RoomDto(hotel.Id, RoomType.Deluxe, 2),
            new RoomDto(hotel.Id, RoomType.Single, 1)
        };

        var response = await client.GetAsync($"api/hotels/{hotel.Id}/Rooms");
        var actual = await response.Content.ReadFromJsonAsync<List<RoomDto>>();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(expected, actual);
        });
    }

    [Fact]
    public async Task GetRoomsFromAndToReturns200AndAvailableRooms()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Foo");
        _ = context.Add(hotel);
        var booking = Booking.Create(hotel.Id, hotel.Rooms.First().Id, new DateTime(2024, 01, 01), new DateTime(2024, 01, 07));
        _ = context.Add(booking);
        _ = context.SaveChanges();

        var response = await client.GetAsync($"api/hotels/{hotel.Id}/Rooms?From=2024-01-01&To=2024-01-07");
        var actual = await response.Content.ReadFromJsonAsync<List<RoomDto>>();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(actual);
            Assert.Equivalent(5, actual.Count);
        });
    }

    [Fact]
    public async Task GetRoomsReturns404WhenNoHotelFound()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var response = await client.GetAsync($"api/hotels/{Guid.NewGuid()}/Rooms");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
