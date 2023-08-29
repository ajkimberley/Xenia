using ScreenMedia.Xenia.Bookings.Domain.Enums;

namespace ScreenMedia.Xenia.WebApi.Dtos;

public record BookingDto(Guid HotelId, Guid RoomId, string BookerName, string BookerEmail, DateTime From, DateTime To, BookingState? BookingState = null);
