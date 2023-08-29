namespace ScreenMedia.Xenia.Bookings.Domain.Exceptions;

public class NoVacanciesAvailableException : Exception
{
    public NoVacanciesAvailableException(string message) : base(message)
    {
    }
}
