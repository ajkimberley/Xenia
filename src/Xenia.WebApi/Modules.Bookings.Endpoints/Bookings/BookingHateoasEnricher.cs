using Common.Endpoints.Hateoas;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using Modules.Bookings.Application;

namespace Modules.Bookings.Endpoints.Bookings;

// ReSharper disable once ClassNeverInstantiated.Global
public class BookingHateoasEnricher : HateoasEnricherBase<BookingDto>
{
    public override object Enrich(BookingDto dto, LinkGenerator linkGenerator, HttpContext httpContext) =>
        new
        {
            Booking = dto,
            Links = new List<LinkDto>
            {
                new(
                    linkGenerator.GetPathByName(httpContext, EndpointNames.RetrieveABooking, new { id = dto.Id })!,
                    "self",
                    HttpMethod.Get.Method)
            }
        };
}