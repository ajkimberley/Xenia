using Common.Application;
using ErrorOr;

using MediatR;

using Modules.HotelAdmin.Domain.Hotels;

namespace Modules.HotelAdmin.Application.GetAvailableRooms;

public record GetAvailableRoomsQuery(Guid HotelId, DateTime From, DateTime To) : IRequest<ErrorOr<List<RoomDto>>>;

public class GetAvailableRoomsHandler(IHotelRepository hotelRepo) : IRequestHandler<GetAvailableRoomsQuery, ErrorOr<List<RoomDto>>>
{
    public async Task<ErrorOr<List<RoomDto>>> Handle(GetAvailableRoomsQuery query, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepo.GetHotelWithAvailableRooms(query.HotelId, query.From, query.To);
        if (hotel is null) return RestErrors.ResourceNotFoundError;

        var availableRooms = hotel?.Rooms;
        return availableRooms != null ? availableRooms.Select(r => new RoomDto(r.Hotel.Id, r.Name, r.Capacity)).ToList() : [];
    }
}