namespace Xenia.Bookings.Domain;
public interface IUnitOfWork
{
    Task<int> CompleteAsync(CancellationToken cancellationToken);
}
