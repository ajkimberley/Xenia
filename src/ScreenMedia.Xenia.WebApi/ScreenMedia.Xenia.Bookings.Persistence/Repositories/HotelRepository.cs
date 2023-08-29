using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Repositories;

namespace ScreenMedia.Xenia.Bookings.Persistence.Repositories;
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(BookingContext context) : base(context) { }

    public async Task<IEnumerable<Hotel>> GetAllAsync(string? name) =>
        await _context.Set<Hotel>()
                      .Where(h => h.Name == name)
                      .ToListAsync();

    public async Task<Hotel?> GetHotelWithRoomsByIdAsync(Guid id) =>
        await _context.Set<Hotel>()
                      .Include(h => h.Rooms)
                      .SingleOrDefaultAsync(h => h.Id == id);
}
