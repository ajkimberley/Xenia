using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.Bookings.Domain.Entities;

public class Booking : Entity
{
    private Booking(Guid id, Guid hotelId, Guid roomId, BookingState state, DateTime from, DateTime to)
    {
        Id = id;
        HotelId = hotelId;
        RoomId = roomId;
        State = state;
        From = from;
        To = to;
    }

    public Guid HotelId { get; private set; }
    public Guid RoomId { get; private set; }
    public BookingState State { get; private set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }

    public static Booking Create(Guid hotelId, Guid roomId, DateTime from, DateTime to)
        => new(Guid.NewGuid(), hotelId, roomId, BookingState.Requested, from, to);
}
