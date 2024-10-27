using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;

namespace Xenia.Application.Queries;

public record GetBookingsQuery(string? BookingReference = null) : IRequest<IEnumerable<BookingDto>>;

public class GetBookingsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookingsQuery, IEnumerable<BookingDto>>
{
    public async Task<IEnumerable<BookingDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings =
            request.BookingReference != null
                ? await unitOfWork.Bookings.GetAllAsync(request.BookingReference)
                : await unitOfWork.Bookings.GetAllAsync();
        var dtos = bookings.Select(b =>
            new BookingDto(b.HotelId, b.RoomType, b.BookerName, b.BookerEmail, b.From, b.To, b.State, b.Id, b.Reference));
        return dtos;
    }
}