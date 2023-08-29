using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.WebApi.Dtos;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record BookRoomCommand(Guid HotelId,
                              Guid RoomId,
                              string BookerName,
                              string BookerEmail,
                              DateTime From,
                              DateTime To) : IRequest<BookingResponseDto>;

public class BookRoomHandler : IRequestHandler<BookRoomCommand, BookingResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BookRoomHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<BookingResponseDto> Handle(BookRoomCommand request, CancellationToken cancellationToken)
    {
        var newBooking = Booking.Create(request.HotelId, request.RoomId, request.BookerName, request.BookerEmail, request.From, request.To);
        await _unitOfWork.Bookings.AddAsync(newBooking);
        try
        {
            _ = await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        return new BookingResponseDto();
    }
}
