using Common.Domain;

using MediatR;

using Modules.Bookings.Domain;
using Modules.HotelAdmin.Domain.Hotels;

namespace Modules.Utilities.Application.Seeding;

public record UnseedDataCommand : IRequest;

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
