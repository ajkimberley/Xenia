using ErrorOr;

using Xenia.Bookings.Domain.Enums;
using Xenia.Bookings.Domain.Errors;
using Xenia.Common;

namespace Xenia.Bookings.Domain.Entities;

public class Hotel : Entity
{
    private Hotel(Guid id, string name)
    {
        Id = id;
        Name = name;
        Rooms = new List<Room>();
    }

    public string Name { get; private set; }
    
    public ICollection<Room> Rooms { get; private set; }

    private IEnumerable<Room> GetAvailableRooms(DateTime from, DateTime to)
        => Rooms.Where(r => r.IsAvailable(from, to));

    private IEnumerable<Room> GetAvailableRooms(DateTime from, DateTime to, RoomType roomType) =>
        GetAvailableRooms(from, to).Where(r => r.Type == roomType);
    
    public ErrorOr<Booking> BookRoom(string name, string email, DateTime from, DateTime to, RoomType roomType)
    {
        var availableRooms = GetAvailableRooms(from, to, roomType).ToArray();
        
        if (availableRooms.Length == 0) return HotelErrors.NoVacancyAvailable;
        // return new HotelErrors(
        //     $"There are no vacancies for a {roomType} room between dates {from:u} and {to:u}.");
        
        var availableRoom = availableRooms.First();
        var booking = Booking.Create(Id, roomType, name, email, from, to, availableRoom);
        availableRoom.AddBooking(booking);
        
        return booking;
    }

    public static Hotel Create(string name)
    {
        var newHotel = new Hotel(Guid.NewGuid(), name);
        var rooms = new List<Room>
        {
            Room.CreateSingle(newHotel, 1),
            Room.CreateSingle(newHotel, 2),
            Room.CreateDouble(newHotel, 3),
            Room.CreateDouble(newHotel, 4),
            Room.CreateDeluxe(newHotel, 5),
            Room.CreateDeluxe(newHotel, 6),
        };
        newHotel.Rooms = rooms;
        return newHotel;
    }
}
