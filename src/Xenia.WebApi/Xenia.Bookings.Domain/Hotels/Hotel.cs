using ErrorOr;

using Xenia.Bookings.Domain.Bookings;
using Xenia.Common;

namespace Xenia.Bookings.Domain.Hotels;

public class Hotel : Entity
{
    private Hotel(Guid id, string name)
    {
        Id = id;
        Name = name;
        Rooms = new List<RoomType>();
    }

    public string Name { get; private set; }
    
    public ICollection<RoomType> Rooms { get; private set; }

    private IEnumerable<RoomType> GetAvailableRooms(DateTime from, DateTime to)
        => Rooms.Where(r => r.IsAvailable(from, to));

    private IEnumerable<RoomType> GetAvailableRooms(DateTime from, DateTime to, string name) =>
        GetAvailableRooms(from, to).Where(r => r.Name == name);
    
    public ErrorOr<Booking> BookRoom(string name, string email, DateTime from, DateTime to, string roomType)
    {
        var availableRooms = GetAvailableRooms(from, to, roomType).ToArray();
        if (availableRooms.Length == 0) return HotelErrors.NoVacancyAvailable;
        
        var availableRoom = availableRooms.First();
        var booking = Booking.Create(Id, name, email, from, to, availableRoom);
        availableRoom.AddBooking(booking);
        
        return booking;
    }

    public static Hotel Create(string name)
    {
        var newHotel = new Hotel(Guid.NewGuid(), name);
        var rooms = new List<RoomType>
        {
            RoomType.CreateSingle(newHotel),
            RoomType.CreateSingle(newHotel),
            RoomType.CreateDouble(newHotel),
            RoomType.CreateDouble(newHotel),
            RoomType.CreateDeluxe(newHotel),
            RoomType.CreateDeluxe(newHotel),
        };
        newHotel.Rooms = rooms;
        return newHotel;
    }
}
