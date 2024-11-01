using System.Net;
using System.Runtime.InteropServices.JavaScript;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Enums;
using Xenia.Bookings.Persistence;

namespace Xenia.WebApi.IntegrationTests;

[Collection("WebApi Collection")]
public sealed class RoomControllerTests(XeniaWebApplicationFactory<Program> applicationFactory)
{
    public static IEnumerable<object[]> QueryData()
    {
        yield return [new DateTime(2024, 1, 1), new DateTime(2024, 1, 7), 5];
        yield return [new DateTime(2024, 1, 2), new DateTime(2024, 1, 7), 5];
        yield return [new DateTime(2024, 1, 3), new DateTime(2024, 1, 7), 5];
        yield return [new DateTime(2024, 1, 4), new DateTime(2024, 1, 7), 5];
        yield return [new DateTime(2024, 1, 5), new DateTime(2024, 1, 7), 5];
        yield return [new DateTime(2024, 1, 6), new DateTime(2024, 1, 7), 5];
        yield return [new DateTime(2024, 1, 3), new DateTime(2024, 1, 7), 5];
        yield return [new DateTime(2024, 1, 1), new DateTime(2024, 1, 2), 5];
        yield return [new DateTime(2023, 1, 1), new DateTime(2023, 1, 7), 6];
        yield return [new DateTime(2024, 1, 7), new DateTime(2024, 1, 8), 6];
    }

    [Theory]
    [MemberData(nameof(QueryData))]
    public async Task GetRoomsFromAndToReturns200AndAvailableRooms(DateTime from, DateTime to, int expected)
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<BookingContext>()
                                  ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Foo");
        var room = hotel.Rooms.First();
        var booking = Booking.Create(hotel.Id, RoomType.Single, "Joe Bloggs", "j.bloggs@example.com",
            new DateTime(2024, 01, 01), new DateTime(2024, 01, 07), room);
        hotel.Rooms.FirstOrDefault()!.AddBooking(booking);
        _ = context.Add(hotel);
        _ = await context.SaveChangesAsync();

        var response = await client.GetAsync($"api/hotels/{hotel.Id}/Rooms?From={from:u}&To={to:u}");
        var actual = await response.Content.ReadFromJsonAsync<List<RoomDto>>();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(actual);
            Assert.Equivalent(expected, actual.Count);
        });
    }
    
    // [Fact]
    // public async Task GetRoomsFromAndToReturnsErrorWhenNoAvailableRooms()
    // {
    //     var client = _applicationFactory.CreateClient();
    //     using var scope = _applicationFactory.Services.CreateScope();
    //     await using var context = scope.ServiceProvider.GetService<BookingContext>()
    //                               ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");
    //
    //     var hotel = Hotel.Create("Foo");
    //     _ = context.Add(hotel);
    //     _ = await context.SaveChangesAsync();
    //     
    //     var bookingDto = new BookingDto(
    //         hotel.Id,
    //         RoomType.Single,
    //         "Joe Bloggs",
    //         "j.bloggs@example.com",
    //         new DateTime(2024, 1, 1),
    //         new DateTime(2024, 1, 7));
    //
    //     var requestContent = JsonContent.Create(bookingDto);
    //     _ = await client.PostAsync("api/Bookings", requestContent);
    //     _ = await client.PostAsync("api/Bookings", requestContent);
    //     _ = await client.PostAsync("api/Bookings", requestContent);
    //
    //
    //     var from = new DateTime(2024, 01, 01);
    //     var to = new DateTime(2024, 01, 07);
    //     
    //     var response = await client.GetAsync($"api/hotels/{hotel.Id}/Rooms?From={from:u}&To={to:u}");
    //     var actual = await response.Content.ReadFromJsonAsync<List<RoomDto>>();
    //
    //     Assert.Multiple(() =>
    //     {
    //         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //         Assert.NotNull(actual);
    //         //Assert.Equivalent(expected, actual.Count);
    //     });
    // }

    [Fact]
    public async Task GetRoomsReturns404WhenNoHotelFound()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<BookingContext>()
                                  ?? throw new InvalidOperationException(
                                      $"Unable to find instance of {nameof(BookingContext)}");

        var response = await client.GetAsync(
            $"api/hotels/{Guid.NewGuid()}/Rooms" +
            $"?From={new DateTime(2022, 01, 01)}" +
            $"&To={new DateTime(2022, 01, 07)}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetRoomsFailsValidationWhenFromAndToDateNotValid()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<BookingContext>()
                                  ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Foo");

        var response = await client.GetAsync(
            $"api/hotels/{hotel.Id}/Rooms" +
            $"?From={new DateTime(2024, 02, 01):u}" +
            $"&To={new DateTime(2024, 01, 01):u}");
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
