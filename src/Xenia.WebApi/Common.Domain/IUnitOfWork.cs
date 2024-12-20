namespace Common.Domain;
public interface IUnitOfWork
{
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}
