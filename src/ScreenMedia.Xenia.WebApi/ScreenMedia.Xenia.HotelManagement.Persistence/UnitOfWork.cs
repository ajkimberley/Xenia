using ScreenMedia.Xenia.HotelManagement.Domain;
using ScreenMedia.Xenia.HotelManagement.Domain.Repositories;
using ScreenMedia.Xenia.HotelManagement.Persistence.Repositories;

namespace ScreenMedia.Xenia.HotelManagement.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly HotelManagementContext _context;

    public UnitOfWork(HotelManagementContext context)
    {
        _context = context;
        Hotels = new HotelRepository(_context);
    }

    public IHotelRepository Hotels { get; private set; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
