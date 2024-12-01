using Xenia.Bookings.Domain.Bookings;

namespace Xenia.Application.Bookings;

public record BookingDto(
    Guid HotelId,
    string RoomType,
    string BookerName,
    string BookerEmail,
    DateTime From,
    DateTime To,
    BookingState? BookingState = null,
    Guid? Id = null,
    string? Reference = null);