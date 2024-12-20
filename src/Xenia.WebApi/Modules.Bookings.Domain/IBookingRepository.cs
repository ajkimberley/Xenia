using Common.Domain;

namespace Modules.Bookings.Domain;
public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef);
}
