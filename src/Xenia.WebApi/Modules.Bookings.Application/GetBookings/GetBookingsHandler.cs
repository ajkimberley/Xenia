using Common.Domain;

using MediatR;

using Modules.Bookings.Domain;

namespace Modules.Bookings.Application.GetBookings;

public record GetBookingsQuery(string? BookingReference = null) : IRequest<IEnumerable<BookingDto>>;

public class GetBookingsHandler(IUnitOfWork unitOfWork, IBookingRepository bookingRepo) : IRequestHandler<GetBookingsQuery, IEnumerable<BookingDto>>
{
    public async Task<IEnumerable<BookingDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings =
            request.BookingReference != null
                ? await bookingRepo.GetAllAsync(request.BookingReference)
                : await bookingRepo.GetAllAsync();
        var dtos = bookings.Select(b => new BookingDto(b.HotelId, "Room Type Name", b.BookerName, b.BookerEmail, b.From, b.To, b.State, b.Id, b.Reference));
        return dtos;
    }
}