using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.Bookings.Domain.Entities;

public class Booking : Entity
{
    private Booking(Guid id, Guid hotelId, RoomType roomType, string bookerName, string bookerEmail, BookingState state, DateTime from, DateTime to)
    {
        Id = id;
        HotelId = hotelId;
        RoomType = roomType;
        BookerName = bookerName;
        BookerEmail = bookerEmail;
        State = state;
        From = from;
        To = to;
    }

    public Guid HotelId { get; private set; }
    public Guid RoomId { get; private set; }
    public RoomType RoomType { get; private set; }
    public string BookerName { get; set; }
    public string BookerEmail { get; set; }
    public BookingState State { get; private set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }

    public static Booking Create(Guid hotelId, RoomType roomType, string bookerName, string bookerEmail, DateTime from, DateTime to)
        => new(Guid.NewGuid(), hotelId, roomType, bookerName, bookerEmail, BookingState.Requested, from, to);
}
