using Microsoft.EntityFrameworkCore;

using Xenia.Bookings.Domain.Hotels;

namespace Xenia.Bookings.Persistence.Repositories;
public class HotelRepository(BookingContext context) : GenericRepository<Hotel>(context), IHotelRepository
{
    public async Task<IEnumerable<Hotel>> GetAllAsync(string? name) =>
        await Context.Set<Hotel>()
                      .Where(h => h.Name == name)
                      .ToListAsync();

    public async Task<Hotel?> GetHotelWithRoomsByIdAsync(Guid id) =>
        await Context.Set<Hotel>()
                      .Include(h => h.Rooms)
                      .SingleOrDefaultAsync(h => h.Id == id);

    public async Task<Hotel?> GetHotelWithRoomsAndBookingsByIdAsync(Guid id) =>
        await Context.Set<Hotel>()
                      .Include(h => h.Rooms)
                      .ThenInclude(r => r.Bookings)
                      .SingleOrDefaultAsync(h => h.Id == id);
    
    public async Task<Hotel?> GetHotelWithAvailableRooms(Guid id, DateTime from, DateTime to) =>
            await Context.Set<Hotel>()
                             .Include(h => h.Rooms
                                .Where(r => r.Bookings
                                    .All(b => (b.From < from && b.To <= from) || 
                                                     (b.From >= to))))
                             .SingleOrDefaultAsync(h => h.Id == id);
}
