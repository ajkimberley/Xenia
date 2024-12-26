using FastEndpoints;

namespace Modules.Bookings.Endpoints.Bookings;

public record GetBookingRequest
{
    [QueryParam, BindFrom("id")]
    public Guid Id { get; set; }
}