using ScreenMedia.Xenia.HotelManagement.Domain.Entities;
using ScreenMedia.Xenia.HotelManagement.Domain.Repositories;

namespace ScreenMedia.Xenia.HotelManagement.Persistence.Repositories;
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(HotelManagementContext context) : base(context) { }
}
