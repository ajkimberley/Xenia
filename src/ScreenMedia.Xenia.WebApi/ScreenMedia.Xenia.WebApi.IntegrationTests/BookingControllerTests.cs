using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

using ScreenMedia.Xenia.Bookings.Persistence;
using ScreenMedia.Xenia.HotelManagement.Domain.Entities;
using ScreenMedia.Xenia.HotelManagement.Persistence;
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
    public async Task PostBookingReturns201WhenBookingDtoIsValid()
    {
        var client = _applicationFactory.CreateClient();
        var bookingDto = new BookingDto(
            Guid.NewGuid().ToString(),
            "Requested",
            new PersonDto("John", "smith", "jsmith@example.com"),
            new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7),
            new List<RoomRequestDto>() {
                new RoomRequestDto("Double", new List<PersonDto>() {
                    new PersonDto("John", "Smith", "jsmith@example.com") }) });
        var requestContent = JsonContent.Create(bookingDto, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

        var response = await client.PostAsync("api/Bookings", requestContent);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostBookingPersistsBookingWhenBookingDtoIsValid()
    {
        var client = _applicationFactory.CreateClient();
        using var scope = _applicationFactory.Services.CreateScope();
        using var hotelContext = scope.ServiceProvider.GetService<HotelManagementContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(HotelManagementContext)}");
        using var bookingContext = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}"); ;
        var trackedEntity = hotelContext.Hotels.Add(Hotel.Create("Foo"));

        var entityEntry = hotelContext.Add(Hotel.Create("Foo"));
        _ = hotelContext.SaveChanges();

        var createdHotel = entityEntry.Entity;
        var expected = new HotelDto(createdHotel.Name, createdHotel.Id);

        var bookingDto = new BookingDto(
            Guid.NewGuid().ToString(),
            "Requested",
            new PersonDto("John", "smith", "jsmith@example.com"),
            new DateTime(2024, 1, 1),
            new DateTime(2024, 1, 7),
            new List<RoomRequestDto>() {
                        new RoomRequestDto("Double", new List<PersonDto>() {
                            new PersonDto("John", "Smith", "jsmith@example.com") }) });

        var requestContent = JsonContent.Create(bookingDto, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
        var response = await client.PostAsync("api/Bookings", requestContent);
        _ = response.EnsureSuccessStatusCode();

        var actual = await response.Content.ReadFromJsonAsync<BookingResponseDto>();

        Assert.NotEmpty(bookingContext.Bookings);
    }
}
