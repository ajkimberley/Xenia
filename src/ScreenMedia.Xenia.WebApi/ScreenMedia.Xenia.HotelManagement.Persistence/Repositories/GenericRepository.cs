using Microsoft.EntityFrameworkCore;

using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.HotelManagement.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly HotelManagementContext _context;

    public GenericRepository(HotelManagementContext context) => _context = context;

    public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
}
