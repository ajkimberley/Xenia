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
                              DateTime To) : IRequest<BookingResponseDto>;

public class BookRoomHandler : IRequestHandler<BookRoomCommand, BookingResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public BookRoomHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<BookingResponseDto> Handle(BookRoomCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetHotelWithRoomsAndBookingsByIdAsync(request.HotelId)
            ?? throw new ResourceNotFoundException($"Unable to find hotel with Id {request.HotelId}.");

        try
        {
            var newBooking = Booking.Create(request.HotelId, request.RoomType, request.BookerName, request.BookerEmail, request.From, request.To);
            hotel.BookRoom(newBooking);
            await _unitOfWork.Bookings.AddAsync(newBooking);
            _ = await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
#pragma warning disable CA2200 // Rethrow to preserve stack details
            throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
        }
        return new BookingResponseDto();
    }
}
