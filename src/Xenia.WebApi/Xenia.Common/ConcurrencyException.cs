namespace Xenia.Common;

public class ConcurrencyException(string message, Exception ex) : Exception(message, ex);