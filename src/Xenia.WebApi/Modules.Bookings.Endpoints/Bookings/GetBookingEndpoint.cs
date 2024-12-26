using ErrorOr;

using FastEndpoints;

using MediatR;

using Modules.Bookings.Application;
using Modules.Bookings.Application.GetBooking;

namespace Modules.Bookings.Endpoints.Bookings;

public class GetBookingEndpoint(ISender mediator) : Ep.Req<GetBookingRequest>.Res<ErrorOr<BookingDto>>
{
    public override void Configure() => 
        Get( "bookings/get-booking");

    public override async Task<ErrorOr<BookingDto>> HandleAsync(GetBookingRequest req, CancellationToken ct)
    {
        var qry = new GetBookingQuery(req.Id);

        return await mediator.Send(qry, ct);
    }
}