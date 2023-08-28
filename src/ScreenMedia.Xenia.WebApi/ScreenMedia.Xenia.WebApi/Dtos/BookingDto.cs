using ScreenMedia.Xenia.Bookings.Domain.Enums;

namespace ScreenMedia.Xenia.WebApi.Dtos;

public record BookingDto(Guid HotelId, Guid RoomId, PersonDto BookedBy, DateTime From, DateTime To, BookingState? BookingState = null);
