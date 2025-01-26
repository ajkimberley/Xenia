using ErrorOr;

using FastEndpoints;

using MediatR;

using Microsoft.AspNetCore.Builder;

using Modules.Bookings.Application.GetBookings;

namespace Modules.Bookings.Endpoints.Bookings.GetBookings;

public class GetBookingsEndpoint(ISender mediator) : Ep.Req<GetBookingsRequest>.Res<ErrorOr<GetBookingsResponse>>
{
    public override void Configure()
    {
        Get( "bookings/get-bookings");
        AllowAnonymous();
        Description(x => x.WithName(EndpointNames.ListAllBookings));
    }

    public override async Task<ErrorOr<GetBookingsResponse>> ExecuteAsync(GetBookingsRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBookingsQuery(), ct);
        return new GetBookingsResponse(result.ToList());
    }
}