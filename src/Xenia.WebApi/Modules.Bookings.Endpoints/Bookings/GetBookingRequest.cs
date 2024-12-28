using FastEndpoints;

namespace Modules.Bookings.Endpoints.Bookings;

public record GetBookingRequest
{
    [BindFrom("id")]
    public Guid Id { get; set; }
}