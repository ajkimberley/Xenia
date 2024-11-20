using System.Collections.ObjectModel;

using Xenia.Bookings.Domain.Exceptions;
using Xenia.Common;

namespace Xenia.Bookings.Domain.Entities;

public class RoomType : Entity
{
    private readonly Collection<Booking> _bookings;

    // TODO: Should it be possible to create a room without a hotel?
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

    public IReadOnlyCollection<Booking> Bookings => _bookings;
    
    public int Capacity => Name switch
    {
        "single" => 1,
        "double" => 2,
        "deluxe" => 2,
        _ => throw new InvalidRoomTypeException($"Room Type \"{Name}\" does not exist.")
    };
    
    public void AddBooking(Booking booking) => _bookings.Add(booking);

    internal bool IsAvailable(DateTime from, DateTime to)
        => Bookings.All(booking => booking.To < from || booking.From > to);

    internal static RoomType CreateSingle(Hotel hotel) => new(Guid.NewGuid(), hotel, "single");

    internal static RoomType CreateDouble(Hotel hotel) => new(Guid.NewGuid(), hotel, "double");

    internal static RoomType CreateDeluxe(Hotel hotel) => new(Guid.NewGuid(), hotel, "deluxe");
}