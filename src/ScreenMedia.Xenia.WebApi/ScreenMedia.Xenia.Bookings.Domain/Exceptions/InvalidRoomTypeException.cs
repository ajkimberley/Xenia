namespace ScreenMedia.Xenia.Bookings.Domain.Exceptions;
public class InvalidRoomTypeException : Exception
{
    public InvalidRoomTypeException(string message) : base(message)
    {
    }
}
