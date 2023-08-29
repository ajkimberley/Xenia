using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.Bookings.Domain.Repositories;
public interface IHotelRepository : IGenericRepository<Hotel>
{
    Task<IEnumerable<Hotel>> GetAllAsync(string? name);
    Task<Hotel?> GetHotelWithRoomsByIdAsync(Guid id);
}
