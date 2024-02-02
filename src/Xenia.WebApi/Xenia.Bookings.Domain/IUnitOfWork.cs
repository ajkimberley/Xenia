using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Bookings.Domain;
public interface IUnitOfWork
{
    IHotelRepository Hotels { get; }
    IBookingRepository Bookings { get; }
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}
