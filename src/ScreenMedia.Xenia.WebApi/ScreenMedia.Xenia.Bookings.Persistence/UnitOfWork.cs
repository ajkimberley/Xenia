using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Repositories;
using ScreenMedia.Xenia.Bookings.Persistence.Repositories;

namespace ScreenMedia.Xenia.Bookings.Persistence;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly BookingContext _context;

    public UnitOfWork(BookingContext context)
    {
        _context = context;
        Hotels = new HotelRepository(_context);
        Bookings = new BookingRepository(_context);
    }

    public IHotelRepository Hotels { get; private set; }
    public IBookingRepository Bookings { get; private set; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
