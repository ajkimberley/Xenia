﻿using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Bookings.Persistence;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.IntegrationTests;

[Collection("WebApi Collection")]
public sealed class BookingControllerTests
{
    private readonly XeniaWebApplicationFactory<Program> _applicationFactory;

    public BookingControllerTests(XeniaWebApplicationFactory<Program> applicationFactory) =>
        _applicationFactory = applicationFactory;

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

        using var bookingContext = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Holiday Bin");

        var entityEntry = bookingContext.Add(hotel);
        _ = await bookingContext.SaveChangesAsync();

        var expected = new HotelDto(hotel.Name, hotel.Id);

        var bookingDto = new BookingDto(
            hotel.Id,
            RoomType.Single,
            "Joe Bloggs",
            "j.bloggs@example.com",
            new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7));

        var requestContent = JsonContent.Create(bookingDto, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
        var response = await client.PostAsync("api/Bookings", requestContent);
        _ = response.EnsureSuccessStatusCode();

        var bookings = bookingContext.Bookings.ToList();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEmpty(bookings);
        });
    }
}
