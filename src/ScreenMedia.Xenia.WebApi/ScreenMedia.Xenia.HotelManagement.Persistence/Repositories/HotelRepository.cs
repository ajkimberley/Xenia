using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.HotelManagement.Domain.Entities;
using ScreenMedia.Xenia.HotelManagement.Domain.Repositories;

namespace ScreenMedia.Xenia.HotelManagement.Persistence.Repositories;
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(HotelManagementContext context) : base(context) { }

    public async Task<IEnumerable<Hotel>> GetAllAsync(string? name) =>
        await _context.Set<Hotel>()
                      .Where(h => h.Name == name)
                      .ToListAsync();
}
