namespace ScreenMedia.Xenia.Domain.Common;

public class ConcurrencyException : Exception
{
    public ConcurrencyException(string message, Exception ex) : base(message, ex)
    {
    }
}