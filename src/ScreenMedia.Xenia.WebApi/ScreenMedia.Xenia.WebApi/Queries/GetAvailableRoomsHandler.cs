using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;
using ScreenMedia.Xenia.WebApi.Utilities;
using ScreenMedia.Xenia.WebApi.Validation;

namespace ScreenMedia.Xenia.WebApi.Queries;

public record GetAvailableRoomsQuery(Guid HotelId, DateTime? From, DateTime? To) : IRequest<Result<List<RoomDto>, ValidationFailed>>;

public class GetAvailableRoomsHandler : IRequestHandler<GetAvailableRoomsQuery, Result<List<RoomDto>, ValidationFailed>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAvailableRoomsHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<List<RoomDto>, ValidationFailed>> Handle(GetAvailableRoomsQuery query, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetHotelWithAvailableRooms(query.HotelId, query.From, query.To)
            ?? throw new ResourceNotFoundException($"Unable to find hotel with Id {query.HotelId}.");
        var availableRooms = hotel?.Rooms;
        return availableRooms != null ? availableRooms.Select(r => new RoomDto(r.Hotel.Id, r.Type, r.Capacity)).ToList() : new List<RoomDto>();
    }
}
