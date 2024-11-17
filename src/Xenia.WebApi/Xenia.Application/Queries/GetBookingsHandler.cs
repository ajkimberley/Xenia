using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Application.Queries;

public record GetBookingsQuery(string? BookingReference = null) : IRequest<IEnumerable<BookingDto>>;

public class GetBookingsHandler(IUnitOfWork unitOfWork, IBookingRepository bookingRepo) : IRequestHandler<GetBookingsQuery, IEnumerable<BookingDto>>
{
    public async Task<IEnumerable<BookingDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings =
            request.BookingReference != null
                ? await bookingRepo.GetAllAsync(request.BookingReference)
                : await bookingRepo.GetAllAsync();
        var dtos = bookings.Select(b =>
            new BookingDto(b.HotelId, b.RoomType, b.BookerName, b.BookerEmail, b.From, b.To, b.State, b.Id, b.Reference));
        return dtos;
    }
}