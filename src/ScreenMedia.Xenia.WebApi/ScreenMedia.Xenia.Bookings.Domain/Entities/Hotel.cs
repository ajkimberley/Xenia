using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.Bookings.Domain.Entities;

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

    public IEnumerable<Room> GetAvailableRooms(DateTime from, DateTime to)
        => Rooms.Where(r => r.IsAvailable(from, to));

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
