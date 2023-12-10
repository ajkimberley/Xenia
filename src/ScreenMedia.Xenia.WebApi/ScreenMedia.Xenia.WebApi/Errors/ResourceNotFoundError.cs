using ScreenMedia.Xenia.Common.Utilities;

namespace ScreenMedia.Xenia.WebApi.Errors;

public class ResourceNotFoundError : Error
{
    public ResourceNotFoundError() : base(id: Guid.NewGuid()) { }
    public ResourceNotFoundError(string message) : base(id: Guid.NewGuid(), message: message) { }
}