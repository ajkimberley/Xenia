using MediatR;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Common;
using ScreenMedia.Xenia.Common.Utilities;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Errors;

namespace ScreenMedia.Xenia.WebApi.Commands;

public record BookRoomCommand(Guid HotelId,
                              RoomType RoomType,
                              string BookerName,
                              string BookerEmail,
                              DateTime From,
                              DateTime To) : IRequest<Result<BookingDto, Error>>;

public class BookRoomHandler : IRequestHandler<BookRoomCommand, Result<BookingDto, Error>>
{
    private readonly IUnitOfWork _unitOfWork;

    public BookRoomHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<BookingDto, Error>> Handle(BookRoomCommand cmd, CancellationToken cancellationToken)
    {
        const int maxRetires = 3;
        var currentTry = 1;
        do
            try
            {
                return await TryBook(cmd, cancellationToken);
            }
            catch (ConcurrencyException)
            {
                currentTry++;
            }
        while (currentTry <= maxRetires);

        return new MaximumRetryError("Unable to book hotel room after maximum number of retries.");
    }

    private async Task<Result<BookingDto, Error>> TryBook(BookRoomCommand cmd, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetHotelWithRoomsAndBookingsByIdAsync(cmd.HotelId);
        if (hotel is null) return new ResourceNotFoundError($"Unable to find hotel with Id {cmd.HotelId}.");
        
        return await
            hotel.BookRoom(cmd.BookerName, cmd.BookerEmail, cmd.From, cmd.To, cmd.RoomType)
            .Map2(OnSuccess, cancellationToken);
    }

    private async Task<BookingDto> OnSuccess(Booking booking, CancellationToken cancellationToken)
    {
        await _unitOfWork.Bookings.AddAsync(booking);
        _ = await _unitOfWork.CompleteAsync(cancellationToken);
        var bookingDto = CreateBookingDto(booking);
        return bookingDto;
    }
    
    private static BookingDto CreateBookingDto(Booking booking) =>
        new(booking.HotelId,
            booking.RoomType,
            booking.BookerName,
            booking.BookerEmail,
            booking.From,
            booking.To,
            booking.State,
            booking.Id,
            booking.Reference);
}
