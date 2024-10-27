using ErrorOr;

using MediatR;

using Xenia.Bookings.Domain;
using Xenia.Common.Dtos;
using Xenia.Common.Errors;

namespace Xenia.Application.Queries;

public record GetAvailableRoomsQuery(Guid HotelId, DateTime From, DateTime To) : IRequest<ErrorOr<List<RoomDto>>>;

public class GetAvailableRoomsHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAvailableRoomsQuery, ErrorOr<List<RoomDto>>>
{
    public async Task<ErrorOr<List<RoomDto>>> Handle(GetAvailableRoomsQuery query, CancellationToken cancellationToken)
    {
        var hotel = await unitOfWork.Hotels.GetHotelWithAvailableRooms(query.HotelId, query.From, query.To);
        if (hotel is null) return RestErrors.ResourceNotFoundError;
        
        var availableRooms = hotel?.Rooms;
        return availableRooms != null ? availableRooms.Select(r => new RoomDto(r.Hotel.Id, r.Type, r.Capacity)).ToList() : new List<RoomDto>();
    }
}
