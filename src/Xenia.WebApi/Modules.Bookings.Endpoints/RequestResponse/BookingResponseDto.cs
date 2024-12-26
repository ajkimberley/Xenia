using Common.Endpoints;

using Modules.Bookings.Application;

namespace Modules.Bookings.Endpoints.RequestResponse;

public record BookingResponseDto(BookingDto BookingDto, List<LinkDto> Links);
