using Microsoft.EntityFrameworkCore;

using Xenia.Common;

namespace Xenia.Bookings.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    internal readonly BookingContext _context;

    public GenericRepository(BookingContext context) => _context = context;

    public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    public void DeleteRange(IEnumerable<T> range) => _context.Set<T>().RemoveRange(range);
}
