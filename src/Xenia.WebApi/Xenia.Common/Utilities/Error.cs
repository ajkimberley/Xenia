namespace Xenia.Common.Utilities;

public abstract class Error
{
    protected Error(Guid id) => Id = id;
    protected Error(Guid id, string message) { Id = id; Message = message; }

    public Guid Id { get; private set; }
    public string? Message { get; private set; }
}