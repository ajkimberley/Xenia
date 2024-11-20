using ErrorOr;

using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Application.Queries;

public record GetBookingQuery(Guid Id) : IRequest<ErrorOr<BookingDto>>;

public class GetBookingHandler(IBookingRepository bookingRepo) : IRequestHandler<GetBookingQuery, ErrorOr<BookingDto>>
{
    public async Task<ErrorOr<BookingDto>> Handle(GetBookingQuery query, CancellationToken cancellationToken) =>
        (await bookingRepo.GetByIdAsync(query.Id))
        .Then(booking =>
        {
            var bookingDto = new BookingDto(booking.HotelId, booking.RoomType.Name, booking.BookerName, booking.BookerEmail,
                booking.From, booking.To, booking.State, booking.Id);
            return bookingDto;
        });
}