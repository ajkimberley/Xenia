using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record PlaceBookingCommand(Guid HotelId,
                                  Guid RoomId,
                                  PersonDto BookedBy,
                                  DateTime From,
                                  DateTime To) : IRequest<BookingResponseDto>;

public class BookRoom : IRequestHandler<PlaceBookingCommand, BookingResponseDto>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public BookRoom(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<BookingResponseDto> Handle(PlaceBookingCommand request, CancellationToken cancellationToken)
    {
        var newBooking = Booking.Create(request.HotelId, request.RoomId, request.From, request.To);
        await _unitOfWork.Bookings.AddAsync(newBooking);
        _ = await _unitOfWork.CompleteAsync();
        return new BookingResponseDto();
    }
}
