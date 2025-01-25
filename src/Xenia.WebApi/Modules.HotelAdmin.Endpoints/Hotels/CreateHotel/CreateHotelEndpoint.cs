using FastEndpoints;

using MediatR;

using Microsoft.AspNetCore.Builder;

using Modules.HotelAdmin.Application.CreateHotel;

namespace Modules.HotelAdmin.Endpoints.Hotels.CreateHotel;

public class CreateHotelEndpoint(ISender mediator) : Ep.Req<CreateHotelRequest>.Res<CreateHotelResponse>
{
    public override void Configure()
    {
        Post("hotels/");
        AllowAnonymous();
        Description(x => x.WithName(EndpointNames.CreateHotel));
    }

    public override async Task HandleAsync(CreateHotelRequest req, CancellationToken ct)
    {
        var dto = await mediator.Send(new CreateHotelCommand(req.Name), ct);
        var createHotelResponse = new CreateHotelResponse { Id = dto.Id, Name = dto.Name };
        await SendCreatedAtAsync(EndpointNames.GetHotel, new { id = dto.Id }, createHotelResponse, cancellation: ct);
    }
}