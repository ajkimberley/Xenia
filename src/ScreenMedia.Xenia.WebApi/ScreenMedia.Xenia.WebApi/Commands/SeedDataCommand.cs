﻿using MediatR;

using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record SeedDataCommand() : IRequest<SeedDataDto>;

public class SeedDataHandler : IRequestHandler<SeedDataCommand, SeedDataDto>
{
    private readonly IMediator _mediator;
    public SeedDataHandler(IMediator mediator) => _mediator = mediator;

    public async Task<SeedDataDto> Handle(SeedDataCommand cmd, CancellationToken cancellation)
    {
        var createHotelCommands = new List<CreateHotelCommand>()
        {
            new("Travel Bodge"),
            new("Mediocre Inn"),
            new("Holiday Bin")
        };

        var createdHotels = new List<HotelDto>();
        foreach (var c in createHotelCommands)
        {
            var createdHotel = await _mediator.Send(c, cancellation);
            createdHotels.Add(createdHotel);
        }

        return new SeedDataDto(createdHotels.ToList());
    }
}
