using ScreenMedia.Xenia.HotelManagement.Domain.Entities;
using ScreenMedia.Xenia.HotelManagement.Domain.Repositories;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeHotelRepository : FakeGenericRepository<Hotel>, IHotelRepository
{
    public Task<IEnumerable<Hotel>> GetAllAsync(string? name) =>
        Task.FromResult(_list.Where(h => h.Name == name));
}
