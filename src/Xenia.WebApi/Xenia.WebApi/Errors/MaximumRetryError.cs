using ErrorOr;

namespace Xenia.WebApi.Errors;

public static class DatabaseErrors
{
    internal static Error MaximumRetryError = Error.Unexpected(
        code: "Database.MaximumRetryReached",
        description: "Maximum number of retries to the Database has been reached");
}