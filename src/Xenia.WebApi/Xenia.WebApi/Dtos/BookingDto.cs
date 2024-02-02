using Xenia.Bookings.Domain.Enums;

namespace Xenia.WebApi.Dtos;

public record BookingDto(Guid HotelId, RoomType RoomType, string BookerName, string BookerEmail, DateTime From, DateTime To, BookingState? BookingState = null, Guid? Id = null, string? Reference = null);
