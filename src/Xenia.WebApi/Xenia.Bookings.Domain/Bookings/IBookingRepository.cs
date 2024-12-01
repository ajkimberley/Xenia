using Xenia.Common;

namespace Xenia.Bookings.Domain.Bookings;
public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef);
}
