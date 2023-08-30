using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Exceptions;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record BookRoomCommand(Guid HotelId,
                              RoomType RoomType,
                              string BookerName,
                              string BookerEmail,
                              DateTime From,
                              DateTime To) : IRequest<BookingDto>;

public class BookRoomHandler : IRequestHandler<BookRoomCommand, BookingDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BookRoomHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<BookingDto> Handle(BookRoomCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetHotelWithRoomsAndBookingsByIdAsync(request.HotelId)
            ?? throw new ResourceNotFoundException($"Unable to find hotel with Id {request.HotelId}.");

        var newBooking = Booking.Create(request.HotelId, request.RoomType, request.BookerName, request.BookerEmail, request.From, request.To);
        hotel.BookRoom(newBooking);
        await _unitOfWork.Bookings.AddAsync(newBooking);
        _ = await _unitOfWork.CompleteAsync();
        var bookingDto = new BookingDto(hotel.Id, newBooking.RoomType, newBooking.BookerName, newBooking.BookerEmail, newBooking.From, newBooking.To, newBooking.State, newBooking.Id, newBooking.Reference);
        return bookingDto;
    }
}
