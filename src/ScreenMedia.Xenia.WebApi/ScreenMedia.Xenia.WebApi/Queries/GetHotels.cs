using MediatR;

using ScreenMedia.Xenia.HotelManagement.Domain;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Queries;

public record GetHotelsQuery() : IRequest<IEnumerable<HotelDto>>;

public class GetHotels : IRequestHandler<GetHotelsQuery, IEnumerable<HotelDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetHotels(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _unitOfWork.Hotels.GetAllAsync();
        var dtos = hotels.Select(hotel => new HotelDto(hotel.Name));
        return dtos;
    }
}
