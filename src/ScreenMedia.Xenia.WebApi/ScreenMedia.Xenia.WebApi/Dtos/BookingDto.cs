namespace ScreenMedia.Xenia.WebApi.Dtos;

public record BookingDto(string HotelId, string State, PersonDto BookedBy, DateTime From, DateTime To, RoomRequestDto RoomRequests);
