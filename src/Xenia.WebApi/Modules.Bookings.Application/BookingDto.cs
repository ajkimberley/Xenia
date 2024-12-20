using Modules.Bookings.Domain;

namespace Modules.Bookings.Application;

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