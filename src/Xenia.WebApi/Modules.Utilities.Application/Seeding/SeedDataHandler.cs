﻿using MediatR;

using Modules.HotelAdmin.Application;
using Modules.HotelAdmin.Application.CreateHotel;

namespace Modules.Utilities.Application.Seeding;

public record SeedDataCommand : IRequest<SeedDataDto>;

public class SeedDataHandler(ISender mediator) : IRequestHandler<SeedDataCommand, SeedDataDto>
{
    public async Task<SeedDataDto> Handle(SeedDataCommand cmd, CancellationToken cancellation)
    {
        var createHotelCommands = new List<CreateHotelCommand>
        {
            new("Travel Bodge"),
            new("Mediocre Inn"),
            new("Holiday Bin")
        };

        var createdHotels = new List<HotelDto>();
        foreach (var c in createHotelCommands)
        {
            var createdHotel = await mediator.Send(c, cancellation);
            createdHotels.Add(createdHotel);
        }

        return new SeedDataDto(createdHotels.ToList());
    }
}
