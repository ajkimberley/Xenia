using ScreenMedia.Xenia.HotelManagement.Domain.Enums;

namespace ScreenMedia.Xenia.WebApi.Dtos;

public record RoomDto(Guid HotelId, RoomType RoomType, int Capacity);
