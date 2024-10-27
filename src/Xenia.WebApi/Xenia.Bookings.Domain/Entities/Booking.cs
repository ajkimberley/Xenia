using Xenia.Bookings.Domain.Enums;
using Xenia.Common;
using Xenia.Common.Utilities;

namespace Xenia.Bookings.Domain.Entities;

public class Booking : Entity
{
    private Booking(Guid id, Guid hotelId, RoomType roomType, string bookerName, string bookerEmail, BookingState state,
        DateTime from, DateTime to)
    {
        Id = id;
        Reference = Crockbase32.EncodeByteString(id.ToString());
        HotelId = hotelId;
        RoomType = roomType;
        BookerName = bookerName;
        BookerEmail = bookerEmail;
        State = state;
        From = from;
        To = to;
    }

    public Guid HotelId { get; private set; }
    
    public string Reference { get; private set; }
    public string BookerName { get; set; }
    public string BookerEmail { get; set; }
    public RoomType RoomType { get; private set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    
    public BookingState State { get; private set; }
    
    public Room? Room { get; private set; }

    public static Booking Create(Guid hotelId, RoomType roomType, string bookerName, string bookerEmail, DateTime from, DateTime to, Room room)
        => new(Guid.NewGuid(), hotelId, roomType, bookerName, bookerEmail, BookingState.Reserved, from, to) { Room = room };
}
