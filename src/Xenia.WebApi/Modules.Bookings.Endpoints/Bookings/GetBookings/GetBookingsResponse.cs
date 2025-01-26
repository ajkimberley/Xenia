using Modules.Bookings.Application;

namespace Modules.Bookings.Endpoints.Bookings.GetBookings;

public record GetBookingsResponse(IList<BookingDto> Bookings);