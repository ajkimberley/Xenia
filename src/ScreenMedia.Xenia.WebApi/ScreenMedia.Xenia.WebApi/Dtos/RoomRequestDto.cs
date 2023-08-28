namespace ScreenMedia.Xenia.WebApi.Dtos;

public record RoomRequestDto(string RoomType, List<PersonDto> guests);
