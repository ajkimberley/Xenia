using Common.Domain;
using Common.Utilities;

namespace Modules.Bookings.Domain;

public class Booking : Entity
{
    private Booking() {}
    
    private Booking(Guid id, Guid hotelId, string bookerName, string bookerEmail, BookingState state,
        DateTime from, DateTime to, string roomType)
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
    public string BookerName { get; private set; }
    public string BookerEmail { get; private set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    public BookingState State { get; private set; }
    public string RoomType { get; private set; }

    public static Booking Create(Guid hotelId, string bookerName, string bookerEmail, DateTime from, DateTime to, string roomType)
        => new(Guid.NewGuid(), hotelId, bookerName, bookerEmail, BookingState.Reserved, from, to, roomType);
}
