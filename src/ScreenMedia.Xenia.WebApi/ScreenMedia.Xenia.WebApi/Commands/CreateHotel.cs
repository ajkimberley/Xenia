using MediatR;

using ScreenMedia.Xenia.HotelManagement.Domain;
using ScreenMedia.Xenia.HotelManagement.Domain.Entities;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record CreateHotelCommand(string Name) : IRequest<Guid>;

public class CreateHotelHandler : IRequestHandler<CreateHotelCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Guid> Handle(CreateHotelCommand cmd, CancellationToken cancellationToken)
    {
        var hotel = Hotel.Create(cmd.Name);

        await _unitOfWork.Hotels.AddAsync(hotel);
        _ = await _unitOfWork.CompleteAsync();
        return hotel.Id;
    }
}
