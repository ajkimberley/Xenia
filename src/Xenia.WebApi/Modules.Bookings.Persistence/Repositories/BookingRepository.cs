using Common.Persistence;

using Modules.Bookings.Domain;
using ErrorOr;

using Microsoft.EntityFrameworkCore;

namespace Modules.Bookings.Persistence.Repositories;


public class BookingRepository(BookingContext context) : GenericRepository<Booking, BookingContext>(context), IBookingRepository
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

    public override async Task AddAsync(Booking booking)
    {
        await base.AddAsync(booking);
        UpdateVersion(booking.RoomType);
    }
    
    private void UpdateVersion(object roomType)
    {
        var roomEntry = Context.Entry(roomType);
        roomEntry.Property("RowVersion").CurrentValue = DateTime.UtcNow;
    }
}
