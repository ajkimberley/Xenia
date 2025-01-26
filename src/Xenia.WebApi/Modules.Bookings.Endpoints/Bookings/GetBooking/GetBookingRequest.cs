using FastEndpoints;

namespace Modules.Bookings.Endpoints.Bookings.GetBooking;

// ReSharper disable once ClassNeverInstantiated.Global
public record GetBookingRequest
{
    [BindFrom("id")]
    public Guid Id { get; init; }
}