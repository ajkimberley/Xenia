using Microsoft.EntityFrameworkCore;

using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Bookings.Persistence.Repositories;

public class BookingRepository(BookingContext context) : GenericRepository<Booking>(context), IBookingRepository
{
    public async Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef) =>
        await Context.Set<Booking>()
                      .Where(b => b.Reference == bookingRef)
                      .ToListAsync();

    public new async Task AddAsync(Booking booking)
    {
        await base.AddAsync(booking);
        if (booking.Room is not null) UpdateVersion(booking.Room);
    }
    
    private void UpdateVersion(Room room)
    {
        var roomEntry = Context.Entry(room);
        roomEntry.Property("RowVersion").CurrentValue = DateTime.UtcNow;
    }
}
