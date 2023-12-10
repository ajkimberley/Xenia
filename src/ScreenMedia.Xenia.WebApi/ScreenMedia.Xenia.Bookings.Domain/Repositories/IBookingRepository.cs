using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Common;

namespace ScreenMedia.Xenia.Bookings.Domain.Repositories;
public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef);
}
