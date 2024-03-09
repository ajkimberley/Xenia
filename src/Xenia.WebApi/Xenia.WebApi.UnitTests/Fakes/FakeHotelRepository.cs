using System.Reflection;
using System.Runtime.Serialization;

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
        var clonedHotel = (Hotel)FormatterServices.GetUninitializedObject(typeof(Hotel));

        foreach (var property in typeof(Hotel).GetProperties())
            if (property.CanWrite)
                property.SetValue(clonedHotel, property.GetValue(originalHotel));

        var roomsProperty = typeof(Hotel).GetProperty("Rooms", BindingFlags.Instance | BindingFlags.Public);
        roomsProperty?.SetValue(clonedHotel, availableRooms);

        return clonedHotel;
    }
}