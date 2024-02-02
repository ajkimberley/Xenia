using Microsoft.EntityFrameworkCore;

using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Bookings.Persistence.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(BookingContext context) : base(context) { }

    public async Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef) =>
    await _context.Set<Booking>()
                  .Where(b => b.Reference == bookingRef)
                  .ToListAsync();

    public new async Task AddAsync(Booking booking)
    {
        await base.AddAsync(booking);
        if (booking.Room is not null) UpdateVersion(booking.Room);
    }
    
    private void UpdateVersion(Room room)
    {
        var roomEntry = _context.Entry(room);
        roomEntry.Property("RowVersion").CurrentValue = DateTime.UtcNow;
    }
}
