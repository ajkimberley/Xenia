using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

using ScreenMedia.Xenia.HotelManagement.Domain.Entities;
using ScreenMedia.Xenia.HotelManagement.Persistence;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.IntegrationTests;

public sealed class BasicTests : IClassFixture<XeniaWebApplicationFactory<Program>>
{
    private readonly XeniaWebApplicationFactory<Program> _applicationFactory;

    public BasicTests(XeniaWebApplicationFactory<Program> applicationFactory) =>
        _applicationFactory = applicationFactory;

    [Fact]
    public async Task HelloHealthCheck()
    {
        var client = _applicationFactory.CreateClient();
        var expected = "Hello Xenia";

        var response = await client.GetAsync("api/Hello");
        var actual = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Assert.Equal(expected, actual);
        });
    }

    [Fact]
    public async Task PostHotelReturns201WhenHotelDtoIsValid()
    {
        var client = _applicationFactory.CreateClient();
        var requestContent = JsonContent.Create(new HotelDto("Some Hotel Name"), new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        var response = await client.PostAsync("api/Hotels", requestContent);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostHotelReturns400WhenHotelDtoIsInValid()
    {
        var client = _applicationFactory.CreateClient();
        var requestContent = JsonContent.Create("{\"Foo\": \"Bar\"}", new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        var response = await client.PostAsync("api/Hotels", requestContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("Travel Bodge")]
    [InlineData("Mediocre Inn")]
    [InlineData("Holiday Bin")]
    public async Task PostHotelPersistsHotelsWhenDtoIsValid(string hotelName)
    {
        var client = _applicationFactory.CreateClient();
        var requestContent = JsonContent.Create(new HotelDto(hotelName), new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<HotelManagementContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(HotelManagementContext)}");

        var response = await client.PostAsync("api/Hotels", requestContent);
        _ = response.EnsureSuccessStatusCode();

        Assert.NotEmpty(context.Hotels.Where(h => h.Name == hotelName));
    }

    [Fact]
    public async Task GetAllHotelsReturns204WhenDatabaseContainsNoHotels()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<HotelManagementContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(HotelManagementContext)}");
        var hotels = context.Hotels.ToList();
        context.Hotels.RemoveRange(hotels);
        _ = context.SaveChanges();

        var response = await client.GetAsync("api/Hotels");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetAllHotelsReturns200WhenDatabaseContainsHotels()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<HotelManagementContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(HotelManagementContext)}");
        _ = context.Add(Hotel.Create("Foo"));
        _ = context.SaveChanges();

        var response = await client.GetAsync("api/Hotels");
        _ = response.EnsureSuccessStatusCode();

        var actual = await response.Content.ReadFromJsonAsync<IEnumerable<HotelDto>>();
        Assert.Multiple(() =>
        {
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
        });
    }
}
