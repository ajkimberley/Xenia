using Common.Endpoints.Hateoas;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Modules.Bookings.Endpoints.Bookings.GetBookings;

// ReSharper disable once UnusedType.Global
public class BookingsHateoasEnricher(BookingHateoasEnricher bookingEnricher) : HateoasEnricherBase<GetBookingsResponse>
{
    public override object Enrich(GetBookingsResponse dto, LinkGenerator linkGenerator, HttpContext httpContext) =>
        new { Bookings = dto.Bookings.Select(booking => bookingEnricher.Enrich(booking, linkGenerator, httpContext)) };
}