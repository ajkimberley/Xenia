using ScreenMedia.Xenia.Bookings.Domain.Repositories;

namespace ScreenMedia.Xenia.Bookings.Domain;
public interface IUnitOfWork
{
    IBookingRepository Bookings { get; }
    Task<int> CompleteAsync();
}
