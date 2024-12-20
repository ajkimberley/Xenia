﻿using ErrorOr;

using Microsoft.EntityFrameworkCore;

using Xenia.Common;

namespace Xenia.Bookings.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly BookingContext Context;

    protected GenericRepository(BookingContext context) => Context = context;

    public virtual async Task<ErrorOr<T>> GetByIdAsync(Guid id)
    {
        var result = await Context.Set<T>().FindAsync(id);
        return result != null ? result : Error.NotFound();
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();

    public virtual async Task AddAsync(T entity) => await Context.Set<T>().AddAsync(entity);

    public virtual void DeleteRange(IEnumerable<T> range) => Context.Set<T>().RemoveRange(range);
}
