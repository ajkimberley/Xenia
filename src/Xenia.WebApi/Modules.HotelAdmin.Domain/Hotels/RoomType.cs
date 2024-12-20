using System.Collections.ObjectModel;

using Common.Domain;

namespace Modules.HotelAdmin.Domain.Hotels;

public class RoomType : Entity
{
    private readonly Collection<object> _bookings;
    
    private RoomType(Guid id, string name)
    {
        Id = id;
        Name = name;
        _bookings = [];
    }

    private RoomType(Guid id, Hotel hotel, string name)
    {
        Id = id;
        Hotel = hotel;
        Name = name;
        _bookings = [];
    }

    public Hotel Hotel { get; init; } = null!;
    public string Name { get; init; }

    public IReadOnlyCollection<object> Bookings => _bookings;
    
    public int Capacity => Name switch
    {
        "single" => 1,
        "double" => 2,
        "deluxe" => 2,
        _ => throw new InvalidRoomTypeException($"Room Type \"{Name}\" does not exist.")
    };
    
    public void AddBooking(object booking) => _bookings.Add(booking);

    internal bool IsAvailable(DateTime from, DateTime to)
        => throw new NotImplementedException();
            //Bookings.All(booking => booking.To < from || booking.From > to);

    internal static RoomType CreateSingle(Hotel hotel) => new(Guid.NewGuid(), hotel, "single");

    internal static RoomType CreateDouble(Hotel hotel) => new(Guid.NewGuid(), hotel, "double");

    internal static RoomType CreateDeluxe(Hotel hotel) => new(Guid.NewGuid(), hotel, "deluxe");
}