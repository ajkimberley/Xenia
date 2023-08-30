using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;

namespace ScreenMedia.Xenia.WebApi.Queries;

public record GetBookingQuery(Guid Id) : IRequest<BookingDto>;

public class GetBookingHandler : IRequestHandler<GetBookingQuery, BookingDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<BookingDto> Handle(GetBookingQuery query, CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(query.Id)
                          ?? throw new ResourceNotFoundException($"No hotel found with Id {query.Id}.");

        var bookingDto = new BookingDto(booking.HotelId, booking.RoomType, booking.BookerName, booking.BookerEmail, booking.From, booking.To, booking.State, booking.Id);
        return bookingDto;
    }
}
