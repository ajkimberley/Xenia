using MediatR;

using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Application.Commands;

public record UnseedDataCommand() : IRequest;

public class UnseedDataHandler(IUnitOfWork uow, IHotelRepository hotelRepo, IBookingRepository bookingRepo) : IRequestHandler<UnseedDataCommand>
{
    public async Task Handle(UnseedDataCommand cmd, CancellationToken cancellation)
    {
        var hotels = await hotelRepo.GetAllAsync();
        var bookings = await bookingRepo.GetAllAsync();

        hotelRepo.DeleteRange(hotels);
        bookingRepo.DeleteRange(bookings);
        _ = await uow.CompleteAsync(cancellation);
    }
}
