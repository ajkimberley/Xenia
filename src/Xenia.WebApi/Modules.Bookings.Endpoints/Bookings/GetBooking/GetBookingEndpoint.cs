using ErrorOr;

using FastEndpoints;

using MediatR;

using Microsoft.AspNetCore.Builder;

using Modules.Bookings.Application;
using Modules.Bookings.Application.GetBooking;

namespace Modules.Bookings.Endpoints.Bookings;

public sealed class GetBookingEndpoint(ISender mediator) : Ep.Req<GetBookingRequest>.Res<ErrorOr<BookingDto>>
{
    public override void Configure()
    {
        Get( "bookings/get-booking");
        AllowAnonymous();
        Description(x => x.WithName(EndpointNames.GetById));
    }

    public override Task<ErrorOr<BookingDto>> ExecuteAsync(GetBookingRequest req, CancellationToken ct)
        => mediator.Send(new GetBookingQuery(req.Id), ct);
}