using FastEndpoints;

using MediatR;

using Microsoft.AspNetCore.Builder;

using Modules.HotelAdmin.Application.GetHotels;

namespace Modules.HotelAdmin.Endpoints.Hotels.GetHotels;

public sealed class GetHotelsEndpoint(ISender mediator) : Ep.Req<GetHotelsRequest>.Res<GetHotelsResponse>
{
    public override void Configure()
    {
        Get("hotels/");
        AllowAnonymous();
        Description(x => x.WithName(EndpointNames.GetHotels));
    }

    public override async Task HandleAsync(GetHotelsRequest req, CancellationToken ct)
    {
        var qry = new GetHotelsQuery(req.Name);
        var dtos = await mediator.Send(qry, ct);

        var responseHotels = dtos.ToList();
        if (req.Name == null) Response.Hotels = responseHotels;
        
        var dtosList = responseHotels.ToList();
        if (dtosList.Count > 0) Response.Hotels = dtosList;
        else await SendNotFoundAsync(ct);
    }
}