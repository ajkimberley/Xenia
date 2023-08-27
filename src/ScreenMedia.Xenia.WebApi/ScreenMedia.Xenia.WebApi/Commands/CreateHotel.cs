using MediatR;

using ScreenMedia.Xenia.HotelManagement.Domain;
using ScreenMedia.Xenia.HotelManagement.Persistence;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record CreateHotelCommand(string Name) : IRequest;

public class CreateHotelHandler : IRequestHandler<CreateHotelCommand>
{
    private readonly HotelManagementContext _hotelManagementContext;

    public CreateHotelHandler(HotelManagementContext hotelManagementContext) => _hotelManagementContext = hotelManagementContext;
    public async Task Handle(CreateHotelCommand cmd, CancellationToken cancellationToken)
    {
        var hotel = Hotel.Create(cmd.Name);

        _ = _hotelManagementContext.Add(hotel);
        _ = await _hotelManagementContext.SaveChangesAsync();
    }
}
