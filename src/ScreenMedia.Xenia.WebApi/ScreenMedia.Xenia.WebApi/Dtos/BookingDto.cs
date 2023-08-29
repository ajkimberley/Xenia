using ScreenMedia.Xenia.Bookings.Domain.Enums;

namespace ScreenMedia.Xenia.WebApi.Dtos;

public record BookingDto(Guid HotelId, RoomType RoomType, string BookerName, string BookerEmail, DateTime From, DateTime To, BookingState? BookingState = null);
