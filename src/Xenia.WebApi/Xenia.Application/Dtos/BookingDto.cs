using Xenia.Bookings.Domain.Enums;

namespace Xenia.Application.Dtos;

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