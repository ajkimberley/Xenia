using Xenia.Common.Enums;

namespace Xenia.Common.Dtos;

public record BookingDto(
    Guid HotelId,
    RoomType RoomType,
    string BookerName,
    string BookerEmail,
    DateTime From,
    DateTime To,
    BookingState? BookingState = null,
    Guid? Id = null,
    string? Reference = null);