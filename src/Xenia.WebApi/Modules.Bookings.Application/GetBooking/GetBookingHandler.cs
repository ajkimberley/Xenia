using MediatR;
using ErrorOr;

using Modules.Bookings.Domain;

namespace Modules.Bookings.Application.GetBooking;

public record GetBookingQuery(Guid Id) : IRequest<ErrorOr<BookingDto>>;

public class GetBookingHandler(IBookingRepository bookingRepo) : IRequestHandler<GetBookingQuery, ErrorOr<BookingDto>>
{
    public async Task<ErrorOr<BookingDto>> Handle(GetBookingQuery query, CancellationToken cancellationToken) =>
        (await bookingRepo.GetByIdAsync(query.Id))
        .Then(booking =>
        {
            var bookingDto = new BookingDto(booking.HotelId, "Room Type Name", booking.BookerName, booking.BookerEmail,
                booking.From, booking.To, booking.State, booking.Id);
            return bookingDto;
        });
}