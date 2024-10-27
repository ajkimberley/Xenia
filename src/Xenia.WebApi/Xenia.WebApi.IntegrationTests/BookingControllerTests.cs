using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Persistence;
using Xenia.Common.Dtos;
using Xenia.Common.Enums;

using Xunit.Sdk;

namespace Xenia.WebApi.IntegrationTests;

[Collection("WebApi Collection")]
public sealed class BookingControllerTests
{
    private readonly XeniaWebApplicationFactory<Program> _applicationFactory;

    public BookingControllerTests(XeniaWebApplicationFactory<Program> applicationFactory) =>
        _applicationFactory = applicationFactory;

    [Fact]
    public async Task GetHotelsByBookingReferenceReturns200AndCorrectBookingWhenBookingInRepo()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<BookingContext>()
                                  ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = await context.SaveChangesAsync();

        var hotel = Hotel.Create("Travel Bodge");
        var room = hotel.Rooms.First();
        var booking = Booking.Create(hotel.Id, room.Type, "Joe", "Bloggs", new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7), room);
        room.AddBooking(booking);
        _ = context.Add(hotel);
        _ = await context.SaveChangesAsync();

        var expected = new List<BookingDto>()
        {
            new(booking.HotelId, booking.RoomType, booking.BookerName, booking.BookerEmail, booking.From, booking.To,
                booking.State, booking.Id, booking.Reference)
        };

        var response = await client.GetAsync($"api/Bookings?bookingReference={booking.Reference}");
        var actual = await response.Content.ReadFromJsonAsync<List<BookingDto>>();

        Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(expected, actual);
            });
    }

    [Fact]
    public async Task GetBookingReturns200WhenBookingIsInDatabase()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<BookingContext>()
                                  ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = await context.SaveChangesAsync();

        var hotel = Hotel.Create("Travel Bodge");
        var room = hotel.Rooms.First();
        var booking = Booking.Create(hotel.Id, room.Type, "Joe", "Bloggs", new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7), room);
        room.AddBooking(booking);
        _ = context.Add(hotel);
        _ = await context.SaveChangesAsync();

        var expected = new BookingDto(booking.HotelId, booking.RoomType, booking.BookerName, booking.BookerEmail,
            booking.From, booking.To, booking.State, booking.Id);

        var response = await client.GetAsync($"api/Bookings/{booking.Id}");
        var actual = await response.Content.ReadFromJsonAsync<BookingDto>();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected, actual);
        });
    }

    [Fact]
    public async Task GetBookingReturns404WhenBookingIsNotInDatabase()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<BookingContext>()
                                  ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = await context.SaveChangesAsync();

        var response = await client.GetAsync($"api/Bookings/{Guid.NewGuid}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostBookingReturns400WhenBookingDtoIsInvalid()
    {
        var client = _applicationFactory.CreateClient();
        var requestContent = JsonContent.Create("{\"Foo\": \"Bar\"}", new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        var response = await client.PostAsync("api/Bookings", requestContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostBookingReturns201AndPersistsBookingWhenBookingDtoIsValid()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();

        await using var bookingContext = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Holiday Bin");
        bookingContext.Add(hotel);
        
        _ = await bookingContext.SaveChangesAsync();

        var bookingDto = new BookingDto(
            hotel.Id,
            RoomType.Single,
            "Joe Bloggs",
            "j.bloggs@example.com",
            new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7));

        var requestContent = JsonContent.Create(bookingDto);
        var response = await client.PostAsync("api/Bookings", requestContent);
        _ = response.EnsureSuccessStatusCode();

        var bookings = bookingContext.Bookings.ToList();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEmpty(bookings);
        });
    }

    [Fact]
    public async Task PostBookingReturns409ConflictWhenConcurrentOverlappingBookings()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();

        var bookingContext = scope.ServiceProvider.GetService<BookingContext>()
                             ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Holiday Bin");
        bookingContext.Add(hotel);
        await bookingContext.SaveChangesAsync();

        var bookingDto1 = new BookingDto(
            hotel.Id,
            RoomType.Single,
            "Joe Bloggs",
            "j.bloggs@example.com",
            new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7));

        var bookingDto2 = new BookingDto(
            hotel.Id,
            RoomType.Single,
            "Jill Doe",
            "j.doe@example.com",
            new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7));
        
        var bookingDto3 = new BookingDto(
            hotel.Id,
            RoomType.Single,
            "John Smith",
            "j.smith@example.com",
            new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7));

        var requestContent1 = JsonContent.Create(bookingDto1);
        var requestContent2 = JsonContent.Create(bookingDto2);
        var requestContent3 = JsonContent.Create(bookingDto3);

        var request1 = client.PostAsync("api/Bookings", requestContent1);
        var request2 = client.PostAsync("api/Bookings", requestContent2);
        var request3 = client.PostAsync("api/Bookings", requestContent3);

        await Task.WhenAll(request1, request2, request3);

        var statusCodes = new List<HttpStatusCode>
        {
            (await request1).StatusCode,
            (await request2).StatusCode,
            (await request3).StatusCode
        };

        Assert.Multiple(() =>
        {
            Assert.NotEmpty(bookingContext.Bookings.ToList());
            Assert.Equal(1, statusCodes.Count(s => s == HttpStatusCode.Conflict));
            Assert.Equal(2, statusCodes.Count(s => s == HttpStatusCode.Created));
        });
    }
}
