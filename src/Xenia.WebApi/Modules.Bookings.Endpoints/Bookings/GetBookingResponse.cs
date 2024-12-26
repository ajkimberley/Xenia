using Common.Endpoints;

using Modules.Bookings.Application;

namespace Modules.Bookings.Endpoints.Bookings;

public record GetBookingResponse(BookingDto BookingDto, List<LinkDto> Links);