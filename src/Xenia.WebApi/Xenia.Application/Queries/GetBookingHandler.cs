using ErrorOr;

using MediatR;

using Xenia.Bookings.Domain;
using Xenia.Common.Dtos;

namespace Xenia.Application.Queries;

public record GetBookingQuery(Guid Id) : IRequest<ErrorOr<BookingDto>>;

public class GetBookingHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookingQuery, ErrorOr<BookingDto>>
{
    public async Task<ErrorOr<BookingDto>> Handle(GetBookingQuery query, CancellationToken cancellationToken) =>
        (await unitOfWork.Bookings.GetByIdAsync(query.Id))
        .Then(booking =>
        {
            var bookingDto = new BookingDto(booking.HotelId, booking.RoomType, booking.BookerName, booking.BookerEmail,
                booking.From, booking.To, booking.State, booking.Id);
            return bookingDto;
        });
}