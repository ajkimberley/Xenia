using MediatR;

using ScreenMedia.Xenia.HotelManagement.Domain;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;

namespace ScreenMedia.Xenia.WebApi.Queries;

public record GetHotelQuery(Guid Id) : IRequest<HotelDto>;

public class GetHotelHandler : IRequestHandler<GetHotelQuery, HotelDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetHotelHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<HotelDto> Handle(GetHotelQuery query, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(query.Id)
                          ?? throw new ResourceNotFoundException($"No hotel found with Id {query.Id}.");

        var dto = new HotelDto(hotel.Name, hotel.Id);
        return dto;
    }
}
