using MediatR;

using Modules.HotelAdmin.Domain.Hotels;

namespace Modules.HotelAdmin.Application.GetHotels;

public record GetHotelsQuery(string? Name = null) : IRequest<IEnumerable<HotelDto>>;

public class GetHotelsHandler(IHotelRepository hotelRepo) : IRequestHandler<GetHotelsQuery, IEnumerable<HotelDto>>
{
    public async Task<IEnumerable<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels =
            request.Name != null
            ? await hotelRepo.GetAllAsync(request.Name)
            : await hotelRepo.GetAllAsync();
        var dtos = hotels.Select(hotel => new HotelDto(hotel.Name, hotel.Id));
        return dtos;
    }
}
