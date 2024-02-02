using Xenia.Bookings.Domain.Entities;
using Xenia.Common;

namespace Xenia.Bookings.Domain.Repositories;
public interface IHotelRepository : IGenericRepository<Hotel>
{
    Task<IEnumerable<Hotel>> GetAllAsync(string? name);
    Task<Hotel?> GetHotelWithRoomsByIdAsync(Guid id);
    Task<Hotel?> GetHotelWithRoomsAndBookingsByIdAsync(Guid id);
    Task<Hotel?> GetHotelWithAvailableRooms(Guid id, DateTime? start, DateTime? to);
}
