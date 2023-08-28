﻿using ScreenMedia.Xenia.Domain.Common;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeGenericRepository<T> : IGenericRepository<T> where T : Entity
{
    protected readonly List<T> _list = new();

    public Task AddAsync(T entity)
    {
        _list.Add(entity);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult(_list as IEnumerable<T>);

    public Task<T?> GetByIdAsync(Guid id) => Task.FromResult(_list.Where(t => t.Id == id).SingleOrDefault());
}
