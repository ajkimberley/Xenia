using Common.Domain;
using ErrorOr;

using Microsoft.EntityFrameworkCore;

namespace Common.Persistence;

public class GenericRepository<T, TContext> : IGenericRepository<T> where T : class where TContext : DbContext
{
    protected readonly TContext Context;

    protected GenericRepository(TContext context) => Context = context;

    public virtual async Task<ErrorOr<T>> GetByIdAsync(Guid id)
    {
        var result = await Context.Set<T>().FindAsync(id);
        return result != null ? result : Error.NotFound();
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();

    public virtual async Task AddAsync(T entity) => await Context.Set<T>().AddAsync(entity);

    public virtual void DeleteRange(IEnumerable<T> range) => Context.Set<T>().RemoveRange(range);
}
