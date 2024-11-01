using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Persistence;

namespace Xenia.WebApi.IntegrationTests;

[Collection("WebApi Collection")]
public sealed class HotelControllerTests(XeniaWebApplicationFactory<Program> applicationFactory)
{
    [Fact]
    public async Task PostHotelReturns201WhenHotelDtoIsValid()
    {
        var client = applicationFactory.CreateClient();
        var requestContent = JsonContent.Create(new HotelDto("Some Hotel Name"), new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        var response = await client.PostAsync("api/Hotels", requestContent);

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        });
    }

    [Fact]
    public async Task PostHotelReturns400WhenHotelDtoIsInvalid()
    {
        var client = applicationFactory.CreateClient();
        var requestContent = JsonContent.Create("{\"Foo\": \"Bar\"}", new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        var response = await client.PostAsync("api/Hotels", requestContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("Travel Bodge")]
    [InlineData("Mediocre Inn")]
    [InlineData("Holiday Bin")]
    public async Task PostHotelPersistsHotelWhenDtoIsValid(string hotelName)
    {
        var client = applicationFactory.CreateClient();
        var requestContent = JsonContent.Create(new HotelDto(hotelName), new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var response = await client.PostAsync("api/Hotels", requestContent);
        _ = response.EnsureSuccessStatusCode();

        var actual = await response.Content.ReadFromJsonAsync<HotelResponseDto>();

        Assert.NotEmpty(context.Hotels.Where(h => h.Name == hotelName));
    }

    [Fact]
    public async Task GetAllHotelsReturns204WhenDatabaseContainsNoHotels()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = context.SaveChanges();

        var response = await client.GetAsync("api/Hotels");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetAllHotelsReturns200WhenDatabaseContainsHotels()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        _ = context.Add(Hotel.Create("Foo"));
        _ = context.SaveChanges();

        var response = await client.GetAsync("api/Hotels");
        _ = response.EnsureSuccessStatusCode();

        var actual = await response.Content.ReadFromJsonAsync<IEnumerable<HotelDto>>();
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
        });
    }

    [Fact]
    public async Task GetHotelReturns404WhenIdInvalid()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var response = await client.GetAsync("api/Hotels/foo");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetHotelReturns404WhenHotelNotInRepo()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var response = await client.GetAsync($"api/Hotels/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetHotelByIdReturns200AndCorrectHotelWhenHotelInRepo()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = context.SaveChanges();
        var entityEntry = context.Add(Hotel.Create("Foo"));
        _ = context.SaveChanges();

        var createdHotel = entityEntry.Entity;
        var expected = new HotelDto(createdHotel.Name, createdHotel.Id);

        var response = await client.GetAsync($"api/Hotels/{entityEntry.Entity.Id}");
        var actual = await response.Content.ReadFromJsonAsync<HotelDto>();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected, actual);
        });
    }

    [Fact]
    public async Task GetHotelsByNameReturns200AndCorrectHotelsWhenHotelInRepo()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = context.SaveChanges();
        var entityEntry = context.Add(Hotel.Create("Foo"));
        _ = context.SaveChanges();

        var createdHotel = entityEntry.Entity;
        var expected = new List<HotelDto>() { new HotelDto(createdHotel.Name, createdHotel.Id) };

        var response = await client.GetAsync($"api/Hotels?name={entityEntry.Entity.Name}");
        var actual = await response.Content.ReadFromJsonAsync<List<HotelDto>>();

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected, actual);
        });
    }

    [Fact]
    public async Task GetHotelsByNameReturns404WhenNoHotelFoundInRepo()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = context.SaveChanges();

        var response = await client.GetAsync($"api/Hotels?name=foo");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
