using Microsoft.EntityFrameworkCore;

using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.Bookings.Persistence.Repositories;
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(BookingContext context) : base(context) { }

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

    // TODO: Validate against case where from = to
    public async Task<Hotel?> GetHotelWithAvailableRooms(Guid id, DateTime? from, DateTime? to) =>
        from is null || to is null
            ? await GetHotelWithRoomsByIdAsync(id)
            : await Context.Set<Hotel>()
                             .Include(h => h.Rooms
                                .Where(r => r.Bookings
                                    .All(b => (b.From < from && b.To <= from)
                                           || (b.From >= to))))
                             .SingleOrDefaultAsync(h => h.Id == id);

}
