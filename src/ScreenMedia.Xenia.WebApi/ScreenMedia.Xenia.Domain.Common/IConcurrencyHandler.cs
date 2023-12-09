namespace ScreenMedia.Xenia.Domain.Common;

public interface IConcurrencyHandler
{
    Task HandleConcurrencyException(ConcurrencyException ex, CancellationToken cancellationToken);
}