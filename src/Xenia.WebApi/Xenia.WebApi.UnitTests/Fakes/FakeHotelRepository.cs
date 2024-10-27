using System.Reflection;

using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.WebApi.Commands.UnitTests.Fakes;

internal class FakeHotelRepository : FakeGenericRepository<Hotel>, IHotelRepository
{
    public Task<IEnumerable<Hotel>> GetAllAsync(string? name) =>
        Task.FromResult(List.Where(h => h.Name == name));

    public Task<Hotel?> GetHotelWithRoomsAndBookingsByIdAsync(Guid id) =>
        Task.FromResult(List.SingleOrDefault(h => h.Id == id));

    public Task<Hotel?> GetHotelWithRoomsByIdAsync(Guid id) =>
        Task.FromResult(List.SingleOrDefault(h => h.Id == id));

    public Task<Hotel?> GetHotelWithAvailableRooms(Guid id, DateTime from, DateTime to)
    {
        var hotel = List.Single(h => h.Id == id);

        var availableRooms = hotel.Rooms
            .Where(r => r.Bookings
                .All(b => (b.From < from && b.To <= from) || (b.From >= to)))
            .ToList();

        var hotelWithAvailableRooms = CloneHotelWithAvailableRooms(hotel, availableRooms);

        return Task.FromResult(hotelWithAvailableRooms)!;
    }

    private static Hotel CloneHotelWithAvailableRooms(Hotel originalHotel, List<Room> availableRooms)
    {
        // Create a new instance of Hotel using a parameterless constructor
        var clonedHotel = Hotel.Create(originalHotel.Name);

        // Copy each writable property from the originalHotel to clonedHotel
        foreach (var property in typeof(Hotel).GetProperties())
        {
            if (!property.CanWrite) continue;
            var originalValue = property.GetValue(originalHotel);
            property.SetValue(clonedHotel, originalValue);
        }

        // Override the Rooms property with availableRooms
        var roomsProperty = typeof(Hotel).GetProperty("Rooms", BindingFlags.Instance | BindingFlags.Public);
        roomsProperty?.SetValue(clonedHotel, availableRooms);

        return clonedHotel;
    }
}