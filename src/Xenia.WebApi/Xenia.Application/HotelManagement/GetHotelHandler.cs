using ErrorOr;

using MediatR;

using Xenia.Bookings.Domain.Hotels;

namespace Xenia.Application.HotelManagement;

public record GetHotelQuery(Guid Id) : IRequest<ErrorOr<HotelDto>>;

public class GetHotelHandler(IHotelRepository hotelRepo) : IRequestHandler<GetHotelQuery, ErrorOr<HotelDto>>
{
    public async Task<ErrorOr<HotelDto>> Handle(GetHotelQuery query, CancellationToken cancellationToken)
    {
        var hotelResult = await hotelRepo.GetByIdAsync(query.Id);
        if (hotelResult.IsError) return hotelResult.Errors;
        var hotel = hotelResult.Value;
        return new HotelDto(hotel.Name, hotel.Id);
    }
}