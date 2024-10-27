using Xenia.Bookings.Domain.Enums;

namespace Xenia.Application.Dtos;

public record RoomDto(Guid HotelId, RoomType RoomType, int Capacity);
