using ScreenMedia.Xenia.HotelManagement.Domain.Enums;
using ScreenMedia.Xenia.HotelManagement.Domain.Exceptions;

namespace ScreenMedia.Xenia.HotelManagement.Domain.Entities;

public class Room : Entity
{
    private Room(Guid id, int number, RoomType type)
    {
        Id = id;
        Number = number;
        Type = type;
    }

    private Room(Guid id, Hotel hotel, int number, RoomType type)
    {
        Id = id;
        Hotel = hotel;
        Number = number;
        Type = type;
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
