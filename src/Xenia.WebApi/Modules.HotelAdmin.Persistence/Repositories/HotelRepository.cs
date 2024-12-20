using Common.Persistence;

using Microsoft.EntityFrameworkCore;

using Modules.HotelAdmin.Domain.Hotels;

namespace Modules.HotelAdmin.Persistence.Repositories;
public class HotelRepository(HotelAdminContext context) : GenericRepository<Hotel, HotelAdminContext>(context), IHotelRepository
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
                             .Include(h => h.Rooms)
                                // .Where(r => r.Bookings
                                //     .All(b => (b.From < from && b.To <= from) || 
                                //                      (b.From >= to))))
                             .SingleOrDefaultAsync(h => h.Id == id);
}
