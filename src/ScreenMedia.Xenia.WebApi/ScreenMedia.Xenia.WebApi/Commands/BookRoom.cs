using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record PlaceBookingCommand(string HotelId,
                                  string State,
                                  PersonDto BookedBy,
                                  DateTime From,
                                  DateTime To,
                                  RoomRequestDto RoomRequests) : IRequest<BookingResponseDto>;

public class BookRoom : IRequestHandler<PlaceBookingCommand, BookingResponseDto>
{
    private readonly IUnitOfWork unitOfWork;

    public BookRoom(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<BookingResponseDto> Handle(PlaceBookingCommand request, CancellationToken cancellationToken)
    {
        var newBooking = Booking.Create(request.HotelId, request.State);
        await unitOfWork.Bookings.AddAsync(newBooking);
        _ = await unitOfWork.CompleteAsync();
        return new BookingResponseDto();
    }
}
