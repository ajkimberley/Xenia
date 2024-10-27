using Xenia.Common.Enums;

namespace Xenia.Common.Dtos;

public record RoomDto(Guid HotelId, RoomType RoomType, int Capacity);
