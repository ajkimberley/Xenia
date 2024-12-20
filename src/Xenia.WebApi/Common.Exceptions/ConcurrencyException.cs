namespace Common.Exceptions;

public class ConcurrencyException(string message, Exception ex) : Exception(message, ex);