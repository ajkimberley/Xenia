namespace Xenia.WebApi.Dtos;

public record HotelResponseDto(HotelDto CreatedHotel, List<LinkDto> links);
