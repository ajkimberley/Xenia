using ErrorOr;

using MediatR;

using Xenia.Application.Dtos;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Entities;
using Xenia.Common;
using Xenia.Common.Enums;
using Xenia.Common.Errors;

namespace Xenia.Application.Commands;

public record BookRoomCommand(Guid HotelId,
                              RoomType RoomType,
                              string BookerName,
                              string BookerEmail,
                              DateTime From,
                              DateTime To) : IRequest<ErrorOr<BookingDto>>;

public class BookRoomHandler : IRequestHandler<BookRoomCommand, ErrorOr<BookingDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public BookRoomHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<ErrorOr<BookingDto>> Handle(BookRoomCommand cmd, CancellationToken cancellationToken)
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

        return DatabaseErrors.MaximumRetryError;
        //return new MaximumRetryError("Unable to book hotel room after maximum number of retries.");
    }

    private async Task<ErrorOr<BookingDto>> TryBook(BookRoomCommand cmd, CancellationToken cancellationToken)
    {
        var hotel = await _unitOfWork.Hotels.GetHotelWithRoomsAndBookingsByIdAsync(cmd.HotelId);
        if (hotel is null) return RestErrors.ResourceNotFoundError; 
            //return new ResourceNotFoundError($"Unable to find hotel with Id {cmd.HotelId}.");

        var result = 
            await hotel.BookRoom(cmd.BookerName, cmd.BookerEmail, cmd.From, cmd.To, cmd.RoomType)
                .ThenAsync(value => OnSuccess(value, cancellationToken));

        return result;
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
