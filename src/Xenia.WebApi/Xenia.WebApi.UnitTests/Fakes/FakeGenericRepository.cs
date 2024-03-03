using System.Collections.ObjectModel;

using ErrorOr;

using Xenia.Common;

namespace Xenia.WebApi.Commands.UnitTests.Fakes;

internal class FakeGenericRepository<T> : IGenericRepository<T> where T : Entity
{
    protected readonly Collection<T> List = new();

    public virtual Task AddAsync(T entity)
    {
        List.Add(entity);
        return Task.CompletedTask;
    }

    public void DeleteRange(IEnumerable<T> range) => throw new NotImplementedException();

    public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult(List as IEnumerable<T>);

    public Task<ErrorOr<T>> GetByIdAsync(Guid id) =>
        Task.FromResult(List.SingleOrDefault(t => t.Id == id)?.ToErrorOr() ?? Error.NotFound());
}