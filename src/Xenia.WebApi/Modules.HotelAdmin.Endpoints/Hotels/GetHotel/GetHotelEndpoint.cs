using ErrorOr;

using FastEndpoints;

using MediatR;

using Microsoft.AspNetCore.Builder;

using Modules.HotelAdmin.Application;
using Modules.HotelAdmin.Application.GetHotel;

namespace Modules.HotelAdmin.Endpoints.Hotels.GetHotel;

public class GetHotelEndpoint(ISender mediator) : Ep.Req<GetHotelRequest>.Res<ErrorOr<HotelDto>>
{
    public override void Configure()
    {
        Get("hotels/{id}");
        AllowAnonymous();
        Description(x => x.WithName(EndpointNames.GetHotel));
    }

    public override Task<ErrorOr<HotelDto>> ExecuteAsync(GetHotelRequest req, CancellationToken ct)
        => mediator.Send(new GetHotelQuery(req.Id), ct);
}