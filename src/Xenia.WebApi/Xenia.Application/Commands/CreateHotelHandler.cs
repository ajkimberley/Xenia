using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Application.Commands;

public record CreateHotelCommand(string Name) : IRequest<HotelDto>;

public class CreateHotelHandler(IUnitOfWork unitOfWork, IHotelRepository hotelRepo) : IRequestHandler<CreateHotelCommand, HotelDto>
{
    public async Task<HotelDto> Handle(CreateHotelCommand cmd, CancellationToken cancellation)
    {
        var hotel = Hotel.Create(cmd.Name);

        await hotelRepo.AddAsync(hotel);
        _ = await unitOfWork.CompleteAsync(cancellation);

        var bookingDto = new HotelDto(hotel.Name, hotel.Id);
        return bookingDto;
    }
}
