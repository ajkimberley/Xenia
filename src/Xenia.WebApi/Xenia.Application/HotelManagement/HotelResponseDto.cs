using Xenia.Common;

namespace Xenia.Application.HotelManagement;

public record HotelResponseDto(HotelDto CreatedHotel, List<LinkDto> Links);
