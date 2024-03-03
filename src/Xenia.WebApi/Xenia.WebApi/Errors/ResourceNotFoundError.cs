using ErrorOr;

namespace Xenia.WebApi.Errors;

public static class RestErrors
{
    internal static Error ResourceNotFoundError = Error.Unexpected(
        code: "RestError.ResourceNotFound",
        description: "The requested resource cannot be found.");
}