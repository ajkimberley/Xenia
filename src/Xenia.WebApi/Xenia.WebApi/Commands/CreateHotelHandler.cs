using MediatR;

using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Entities;
using Xenia.WebApi.Dtos;

namespace Xenia.WebApi.Commands;

public record CreateHotelCommand(string Name) : IRequest<HotelDto>;

public class CreateHotelHandler : IRequestHandler<CreateHotelCommand, HotelDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<HotelDto> Handle(CreateHotelCommand cmd, CancellationToken cancellation)
    {
        var hotel = Hotel.Create(cmd.Name);

        await _unitOfWork.Hotels.AddAsync(hotel);
        _ = await _unitOfWork.CompleteAsync(cancellation);

        var bookingDto = new HotelDto(hotel.Name, hotel.Id);
        return bookingDto;
    }
}
