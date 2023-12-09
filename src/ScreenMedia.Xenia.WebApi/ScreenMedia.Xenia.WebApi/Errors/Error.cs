namespace ScreenMedia.Xenia.WebApi.Errors;

public abstract class Error
{
    private protected Error(Guid id) => Id = id;
    private protected Error(Guid id, string message) { Id = id; Message = message; }

    public Guid Id { get; private set; }
    public string? Message { get; private set; }
}