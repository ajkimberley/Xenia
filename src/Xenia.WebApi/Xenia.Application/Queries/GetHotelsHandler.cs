using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Application.Queries;

public record GetHotelsQuery(string? Name = null) : IRequest<IEnumerable<HotelDto>>;

public class GetHotelsHandler(IUnitOfWork unitOfWork, IHotelRepository hotelRepo) : IRequestHandler<GetHotelsQuery, IEnumerable<HotelDto>>
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
