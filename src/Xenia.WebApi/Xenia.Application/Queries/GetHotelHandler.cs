using ErrorOr;

using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;

namespace Xenia.Application.Queries;

public record GetHotelQuery(Guid Id) : IRequest<ErrorOr<HotelDto>>;

public class GetHotelHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetHotelQuery, ErrorOr<HotelDto>>
{
    public async Task<ErrorOr<HotelDto>> Handle(GetHotelQuery query, CancellationToken cancellationToken)
    {
        var foo = await unitOfWork.Hotels.GetByIdAsync(query.Id);
        if (!foo.IsError)
        {
            var hotel = foo.Value;
            return new HotelDto(hotel.Name, hotel.Id);
        }
        return foo.Errors;
    }
}