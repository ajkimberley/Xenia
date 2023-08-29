using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;

namespace ScreenMedia.Xenia.WebApi.Queries;

public record GetRoomsQuery(Guid HotelId) : IRequest<List<RoomDto>>;

public class GetRoomsHandler : IRequestHandler<GetRoomsQuery, List<RoomDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoomsHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<List<RoomDto>> Handle(GetRoomsQuery query, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetHotelWithRoomsByIdAsync(query.HotelId)
                          ?? throw new ResourceNotFoundException($"No hotel found with Id {query.HotelId}."); ;
        var rooms = hotel.Rooms;
        var dtos = rooms != null ? rooms.Select(r => new RoomDto(r.Hotel.Id, r.Type, r.Capacity)).ToList() : new List<RoomDto>();
        return dtos;
    }
}
