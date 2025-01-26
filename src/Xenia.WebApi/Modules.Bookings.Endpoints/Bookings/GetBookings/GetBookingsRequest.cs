using FastEndpoints;

namespace Modules.Bookings.Endpoints.Bookings.GetBookings;

public record GetBookingsRequest(DateTime? From, DateTime? To);