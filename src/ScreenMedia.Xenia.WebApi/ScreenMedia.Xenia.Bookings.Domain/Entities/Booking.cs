using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.Bookings.Domain.Entities;

public class Booking : Entity
{
    private Booking(Guid id, Guid hotelId, BookingState state)
    {
        Id = id;
        HotelId = hotelId;
        State = state;
    }

    public Guid HotelId { get; set; }
    public BookingState State { get; set; }

    public static Booking Create(string hotelId, string state)
    {
        if (!Guid.TryParse(hotelId, out var validatedHotelId))
            throw new ArgumentException($"{hotelId} is not a valid Hotel Id", nameof(hotelId));
        if (!Enum.TryParse<BookingState>(state, out var validatedState))
            throw new ArgumentException($"{state} is not a valid Booking State", nameof(state));
        if (validatedState != BookingState.Requested)
            throw new InvalidOperationException($"Cannot create new booking with state: {state}");
        return new Booking(Guid.NewGuid(), validatedHotelId, validatedState);
    }
}
