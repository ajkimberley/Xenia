using Xenia.Bookings.Domain.Enums;

namespace Xenia.WebApi.Dtos;

public record RoomDto(Guid HotelId, RoomType RoomType, int Capacity);
