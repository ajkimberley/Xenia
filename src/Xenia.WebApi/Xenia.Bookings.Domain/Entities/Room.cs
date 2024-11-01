﻿using System.Collections.ObjectModel;

using Xenia.Bookings.Domain.Enums;
using Xenia.Bookings.Domain.Exceptions;
using Xenia.Common;

namespace Xenia.Bookings.Domain.Entities;

public class Room : Entity
{
    private readonly Collection<Booking> _bookings;

    // TODO: Should it be possible to create a room without a hotel?
    private Room(Guid id, int number, RoomType type)
    {
        Id = id;
        Number = number;
        Type = type;
        _bookings = new Collection<Booking>();
    }

    private Room(Guid id, Hotel hotel, int number, RoomType type)
    {
        Id = id;
        Hotel = hotel;
        Number = number;
        Type = type;
        _bookings = new Collection<Booking>();
    }

    public Hotel Hotel { get; init; } = null!;
    public int Number { get; init; }
    public RoomType Type { get; }

    public int Capacity => Type switch
    {
        RoomType.Single => 1,
        RoomType.Double => 2,
        RoomType.Deluxe => 2,
        _ => throw new InvalidRoomTypeException($"Room type {Type} is invalid.")
    };

    public IReadOnlyCollection<Booking> Bookings => _bookings;

    internal bool IsAvailable(DateTime from, DateTime to)
        => Bookings.All(booking => booking.To < from || booking.From > to);

    public void AddBooking(Booking booking) => _bookings.Add(booking);

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