using Common.Endpoints.Hateoas;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using Modules.Bookings.Application;

namespace Modules.Bookings.Endpoints.Bookings;

public class BookingHateoasEnricher : HateoasEnricherBase<BookingDto>
{
    public override object Enrich(BookingDto dto, LinkGenerator linkGenerator, HttpContext httpContext) =>
        new
        {
            dto.HotelId,
            dto.RoomType,
            dto.BookerName,
            dto.BookerEmail,
            dto.From,
            dto.To,
            dto.BookingState,
            dto.Id,
            dto.Reference,
            Links = new Dictionary<string, string> { ["self"] = linkGenerator.GetPathByName(httpContext, "GetBookingById", dto.Id)!, }
        };
}