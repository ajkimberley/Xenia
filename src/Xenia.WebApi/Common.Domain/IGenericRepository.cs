using ErrorOr;

namespace Common.Domain;
public interface IGenericRepository<T> where T : class
{
    Task<ErrorOr<T>> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void DeleteRange(IEnumerable<T> range);
}
