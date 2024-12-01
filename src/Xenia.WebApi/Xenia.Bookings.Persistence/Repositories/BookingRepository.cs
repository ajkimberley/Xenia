using ErrorOr;

using Microsoft.EntityFrameworkCore;

using Xenia.Bookings.Domain.Bookings;
using Xenia.Bookings.Domain.Hotels;

namespace Xenia.Bookings.Persistence.Repositories;

public class BookingRepository(BookingContext context) : GenericRepository<Booking>(context), IBookingRepository
{
    public override async Task<ErrorOr<Booking>> GetByIdAsync(Guid id)
    {
        var result = await Context.Bookings
            .Include(b => b.RoomType)
            .FirstOrDefaultAsync(b => b.Id == id);
        return result == null ? Error.NotFound() : result;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef) =>
        await Context.Set<Booking>()
                      .Where(b => b.Reference == bookingRef)
                      .Include(b => b.RoomType)
                      .ToListAsync();

    public new async Task AddAsync(Booking booking)
    {
        await base.AddAsync(booking);
        UpdateVersion(booking.RoomType);
    }
    
    private void UpdateVersion(RoomType roomType)
    {
        var roomEntry = Context.Entry(roomType);
        roomEntry.Property("RowVersion").CurrentValue = DateTime.UtcNow;
    }
}
