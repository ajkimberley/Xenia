using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Repositories;
using ScreenMedia.Xenia.Bookings.Persistence.Repositories;
using ScreenMedia.Xenia.Common;

namespace ScreenMedia.Xenia.Bookings.Persistence;
public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly BookingContext _context;

    public UnitOfWork(BookingContext context)
    {
        _context = context;
        Hotels = new HotelRepository(_context);
        Bookings = new BookingRepository(_context);
    }

    public IHotelRepository Hotels { get; }
    public IBookingRepository Bookings { get; }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries) await entry.ReloadAsync(cancellationToken);
            throw new ConcurrencyException(ex.Message, ex);
        }
    }

    public void Dispose() => _context.Dispose();
}
