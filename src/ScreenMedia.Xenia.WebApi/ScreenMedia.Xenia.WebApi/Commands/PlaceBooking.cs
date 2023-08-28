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
                                  List<RoomRequestDto> RoomRequests) : IRequest<BookingResponseDto>;

public class PlaceBooking : IRequestHandler<PlaceBookingCommand, BookingResponseDto>
{
    private readonly IUnitOfWork unitOfWork;

    public PlaceBooking(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<BookingResponseDto> Handle(PlaceBookingCommand request, CancellationToken cancellationToken)
    {
        var newBooking = Booking.Create(request.HotelId, request.State);
        await unitOfWork.Bookings.AddAsync(newBooking);
        _ = await unitOfWork.CompleteAsync();
        return new BookingResponseDto();
    }
}
