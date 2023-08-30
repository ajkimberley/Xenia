using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Repositories;

namespace ScreenMedia.Xenia.Bookings.Persistence.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(BookingContext context) : base(context) { }

    public async Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef) =>
    await _context.Set<Booking>()
                  .Where(b => b.Reference == bookingRef)
                  .ToListAsync();
}
