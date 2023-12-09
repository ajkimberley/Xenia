using MediatR;

using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Enums;
using ScreenMedia.Xenia.Bookings.Domain.Exceptions;
using ScreenMedia.Xenia.WebApi.Dtos;
using ScreenMedia.Xenia.WebApi.Errors;
using ScreenMedia.Xenia.WebApi.Utilities;

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
                return await TryBook(cmd);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries) await entry.ReloadAsync(cancellationToken);
                currentTry++;
            }
            catch (NoVacanciesAvailableException ex)
            {
                return new NoVacanciesError(ex.Message);
            }
        while (currentTry <= maxRetires);

        return new MaximumRetryError("Unable to book hotel room after maximum number of retries.");
    }

    private async Task<Result<BookingDto, Error>> TryBook(BookRoomCommand cmd)
    {
        var hotel = await _unitOfWork.Hotels.GetHotelWithRoomsAndBookingsByIdAsync(cmd.HotelId);
        if (hotel is null) return new ResourceNotFoundError($"Unable to find hotel with Id {cmd.HotelId}.");
                
        var booking = CreateBooking(cmd);
        hotel.BookRoom(booking);
                
        await _unitOfWork.Bookings.AddAsync(booking);
        _ = await _unitOfWork.CompleteAsync();

        var bookingDto = CreateBookingDto(booking);
        return bookingDto;
    }

    private static Booking CreateBooking(BookRoomCommand cmd) =>
        Booking.Create(cmd.HotelId, 
            cmd.RoomType, 
            cmd.BookerName,
            cmd.BookerEmail,
            cmd.From, cmd.To);
    
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
