using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;

namespace Xenia.Application.Queries;

public record GetHotelsQuery(string? Name = null) : IRequest<IEnumerable<HotelDto>>;

public class GetHotelsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetHotelsQuery, IEnumerable<HotelDto>>
{
    public async Task<IEnumerable<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels =
            request.Name != null
            ? await unitOfWork.Hotels.GetAllAsync(request.Name)
            : await unitOfWork.Hotels.GetAllAsync();
        var dtos = hotels.Select(hotel => new HotelDto(hotel.Name, hotel.Id));
        return dtos;
    }
}
