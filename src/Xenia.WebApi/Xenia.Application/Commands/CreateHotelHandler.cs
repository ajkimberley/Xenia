using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Entities;

namespace Xenia.Application.Commands;

public record CreateHotelCommand(string Name) : IRequest<HotelDto>;

public class CreateHotelHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateHotelCommand, HotelDto>
{
    public async Task<HotelDto> Handle(CreateHotelCommand cmd, CancellationToken cancellation)
    {
        var hotel = Hotel.Create(cmd.Name);

        await unitOfWork.Hotels.AddAsync(hotel);
        _ = await unitOfWork.CompleteAsync(cancellation);

        var bookingDto = new HotelDto(hotel.Name, hotel.Id);
        return bookingDto;
    }
}
