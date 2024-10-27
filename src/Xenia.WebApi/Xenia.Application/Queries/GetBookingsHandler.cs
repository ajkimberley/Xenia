using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;

namespace Xenia.Application.Queries;

public record GetBookingsQuery(string? BookingReference = null) : IRequest<IEnumerable<BookingDto>>;

public class GetBookingsHandler : IRequestHandler<GetBookingsQuery, IEnumerable<BookingDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingsHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<BookingDto>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings =
            request.BookingReference != null
            ? await _unitOfWork.Bookings.GetAllAsync(request.BookingReference)
            : await _unitOfWork.Bookings.GetAllAsync();
        var dtos = bookings.Select(b =>
            new BookingDto(b.HotelId, b.RoomType, b.BookerName, b.BookerEmail, b.From, b.To, b.State, b.Id, b.Reference));
        return dtos;
    }
}