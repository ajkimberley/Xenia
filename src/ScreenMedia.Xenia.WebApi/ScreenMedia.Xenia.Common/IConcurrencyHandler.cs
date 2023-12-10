namespace ScreenMedia.Xenia.Common;

public interface IConcurrencyHandler
{
    Task HandleConcurrencyException(ConcurrencyException ex, CancellationToken cancellationToken);
}