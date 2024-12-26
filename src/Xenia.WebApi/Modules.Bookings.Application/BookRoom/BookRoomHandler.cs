using Common.Application;
using Common.Domain;
using Common.Exceptions;

using MediatR;
using ErrorOr;

using Modules.Bookings.Domain;

namespace Modules.Bookings.Application.BookRoom;

public record BookRoomCommand(Guid HotelId,
                              string RoomType,
                              string BookerName,
                              string BookerEmail,
                              DateTime From,
                              DateTime To) : IRequest<ErrorOr<BookingDto>>;

public class BookRoomHandler(IUnitOfWork unitOfWork, IBookingRepository bookingRepo) : IRequestHandler<BookRoomCommand, ErrorOr<BookingDto>>
{
    public async Task<ErrorOr<BookingDto>> Handle(BookRoomCommand cmd, CancellationToken cancellationToken)
    {
        // const int maxRetires = 3;
        // var currentTry = 1;
        // do
        //     try
        //     {
        //         return await TryBook(cmd, cancellationToken);
        //     }
        //     catch (ConcurrencyException)
        //     {
        //         currentTry++;
        //     }
        // while (currentTry <= maxRetires);

        return DatabaseErrors.MaximumRetryError;
    }

    private async Task<ErrorOr<BookingDto>> TryBook(BookRoomCommand cmd, CancellationToken cancellationToken)
    {
        // var hotel = await hotelRepo.GetHotelWithRoomsAndBookingsByIdAsync(cmd.HotelId);
        // if (hotel is null) return RestErrors.ResourceNotFoundError;
        //
        // var result = 
        //     await hotel.BookRoom(cmd.BookerName, cmd.BookerEmail, cmd.From, cmd.To, cmd.RoomType)
        //         .ThenAsync(value => OnSuccess(value, cancellationToken));
        //
        // return result;
        throw new NotImplementedException();
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
            "Room Type Name",
            booking.BookerName,
            booking.BookerEmail,
            booking.From,
            booking.To,
            booking.State,
            booking.Id,
            booking.Reference);
}
