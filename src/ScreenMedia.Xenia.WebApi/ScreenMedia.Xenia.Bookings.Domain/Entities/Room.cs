using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Bookings.Domain.Exceptions;
using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.Bookings.Domain.Entities;

public class Room : Entity
{
    private Room(Guid id, int number, RoomType type)
    {
        Id = id;
        Number = number;
        Type = type;
        Bookings = new List<Booking>();
    }

    private Room(Guid id, Hotel hotel, int number, RoomType type)
    {
        Id = id;
        Hotel = hotel;
        Number = number;
        Type = type;
        Bookings = new List<Booking>();
    }

    public Hotel Hotel { get; set; } = null!;
    public int Number { get; set; }
    public RoomType Type { get; private set; }
    public int Capacity => Type switch
    {
        RoomType.Single => 1,
        RoomType.Double => 2,
        RoomType.Deluxe => 2,
        _ => throw new InvalidRoomTypeException($"Room type {Type} is invalid.")
    };
    public ICollection<Booking> Bookings { get; private set; }

    public static Room CreateSingle(Hotel hotel, int number)
    {
        // TODO: Improve validation - i.e., many hotel rooms have number such as 01, 02, etc.
        ValidateRoomNumber(number);
        return new Room(Guid.NewGuid(), hotel, number, RoomType.Single);
    }

    public static Room CreateDouble(Hotel hotel, int number)
    {
        // TODO: Improve validation - i.e., many hotel rooms have number such as 01, 02, etc.
        ValidateRoomNumber(number);
        return new Room(Guid.NewGuid(), hotel, number, RoomType.Double);
    }

    public static Room CreateDeluxe(Hotel hotel, int number)
    {
        // TODO: Improve validation - i.e., many hotel rooms have number such as 01, 02, etc.
        ValidateRoomNumber(number);
        return new Room(Guid.NewGuid(), hotel, number, RoomType.Deluxe);
    }

    private static void ValidateRoomNumber(int number)
    {
        if (number < 0) throw new ArgumentOutOfRangeException($"{nameof(number)}", "Room number must be positive integer");
    }
}
