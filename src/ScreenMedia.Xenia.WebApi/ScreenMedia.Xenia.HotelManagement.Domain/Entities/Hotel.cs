namespace ScreenMedia.Xenia.HotelManagement.Domain.Entities;

// TODO:
// 1. Add more properties
// 2. Substitute or supplement GUID for user-friendly Id
// 3. Validation
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

    public static Hotel Create(string name)
    {
        var newHotel = new Hotel(Guid.NewGuid(), name);
        var rooms = new List<Room>
        {
            Room.CreateSingle(newHotel, 1),
            Room.CreateSingle(newHotel, 2),
            Room.CreateSingle(newHotel, 3),
            Room.CreateSingle(newHotel, 4),
            Room.CreateSingle(newHotel, 5),
            Room.CreateSingle(newHotel, 6),
        };
        newHotel.Rooms = rooms;
        return newHotel;
    }
}
