using Common.Endpoints.Hateoas;

namespace Modules.HotelAdmin.Application;

public record HotelResponseDto(HotelDto CreatedHotel, List<LinkDto> Links);
