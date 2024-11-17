using ErrorOr;

using MediatR;

using Xenia.Application.Dtos;
using Xenia.Application.Errors;
using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Enums;
using Xenia.Bookings.Domain.Repositories;
using Xenia.Common;

namespace Xenia.Application.Commands;

public record BookRoomCommand(Guid HotelId,
                              RoomType RoomType,
                              string BookerName,
                              string BookerEmail,
                              DateTime From,
                              DateTime To) : IRequest<ErrorOr<BookingDto>>;

public class BookRoomHandler(IUnitOfWork unitOfWork, IHotelRepository hotelRepo, IBookingRepository bookingRepo) : IRequestHandler<BookRoomCommand, ErrorOr<BookingDto>>
{
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
    }

    private async Task<ErrorOr<BookingDto>> TryBook(BookRoomCommand cmd, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepo.GetHotelWithRoomsAndBookingsByIdAsync(cmd.HotelId);
        if (hotel is null) return RestErrors.ResourceNotFoundError;

        var result = 
            await hotel.BookRoom(cmd.BookerName, cmd.BookerEmail, cmd.From, cmd.To, cmd.RoomType)
                .ThenAsync(value => OnSuccess(value, cancellationToken));

        return result;
    }

    private async Task<BookingDto> OnSuccess(Booking booking, CancellationToken cancellationToken)
    {
        await bookingRepo.AddAsync(booking);
        _ = await unitOfWork.CompleteAsync(cancellationToken);
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
