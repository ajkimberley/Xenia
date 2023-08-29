using ScreenMedia.Xenia.Bookings.Domain.Repositories;

namespace ScreenMedia.Xenia.Bookings.Domain;
public interface IUnitOfWork
{
    IHotelRepository Hotels { get; }
    IBookingRepository Bookings { get; }
    Task<int> CompleteAsync();
}
