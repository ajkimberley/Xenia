using Common.Domain;

using MediatR;

using Modules.HotelAdmin.Domain.Hotels;

namespace Modules.HotelAdmin.Application.CreateHotel;

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
