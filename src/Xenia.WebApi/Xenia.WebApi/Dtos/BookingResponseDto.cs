namespace Xenia.WebApi.Dtos;

public record BookingResponseDto(BookingDto BookingDto, List<LinkDto> Links);
