using System.Net;

using Xenia.Bookings.Domain.Availabilities;
using Xenia.Bookings.Domain.Hotels;
using Xenia.Bookings.Persistence;

namespace Xenia.WebApi.IntegrationTests;

[Collection("WebApi Collection")]
public sealed class AvailabilityControllerTests(XeniaWebApplicationFactory<Program> applicationFactory)
{
    [Fact]
    public async Task SetAvailabilityReturns201WhenResourceCreated()
    {
        var client = applicationFactory.CreateClient();
        using var scope = applicationFactory.Services.CreateScope();
        await using var context = scope.ServiceProvider.GetService<BookingContext>()
            ?? throw new InvalidOperationException($"Unable to find instance of {nameof(BookingContext)}");

        var hotel = Hotel.Create("Travel Bodge");
        _ = context.Add(hotel);
        _ = await context.SaveChangesAsync();

        var availability = new Availability { AvailableRooms = 1, Date = new DateTime(2024, 1, 1), HotelId = hotel.Id, RoomType = "Single" };
        var requestContent = JsonContent.Create(availability);
        
        var response = await client.PutAsync("api/Availabilities", requestContent);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}