using Xenia.Common.Utilities;

namespace Xenia.WebApi.Errors;

public class MaximumRetryError : Error
{
    internal MaximumRetryError() : base(id: Guid.NewGuid()) { }
    internal MaximumRetryError(string message) : base(id: Guid.NewGuid(), message: message) { }
}