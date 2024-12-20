using Common.Endpoints;

using Modules.Bookings.Application;

namespace Xenia.WebApi.RequestResponse;

public record BookingResponseDto(BookingDto BookingDto, List<LinkDto> Links);
