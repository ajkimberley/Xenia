using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

using ScreenMedia.Xenia.HotelManagement.Persistence;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.IntegrationTests;

public sealed class BasicTests : IClassFixture<XeniaWebApplicationFactory<Program>>, IDisposable
{
    private readonly XeniaWebApplicationFactory<Program> _applicationFactory;

    public BasicTests(XeniaWebApplicationFactory<Program> applicationFactory)
    {
        _applicationFactory = applicationFactory;
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<HotelManagementContext>();
        _ = (context?.Database.EnsureCreated());
    }

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
    public async Task PostHotelPersistsHotelsWhenDtoIsValid()
    {
        var client = _applicationFactory.CreateClient();
        var requestContent = JsonContent.Create(new HotelDto("Some Hotel Name"), new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<HotelManagementContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(HotelManagementContext)}");

        var response = await client.PostAsync("api/Hotels", requestContent);
        _ = response.EnsureSuccessStatusCode();

        Assert.Equal(1, context.Hotels.Count());
    }

    [Fact]
    public async Task PostHotelReturns400WhenHotelDtoIsInValid()
    {
        var client = _applicationFactory.CreateClient();
        var requestContent = JsonContent.Create("{\"Foo\": \"Bar\"}", new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        var response = await client.PostAsync("api/Hotels", requestContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    public void Dispose()
    {
        using var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<HotelManagementContext>();
        _ = (context?.Database.EnsureDeleted());
    }

}