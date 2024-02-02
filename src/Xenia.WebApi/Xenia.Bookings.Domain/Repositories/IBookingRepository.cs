using Xenia.Bookings.Domain.Entities;
using Xenia.Common;

namespace Xenia.Bookings.Domain.Repositories;
public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef);
}
