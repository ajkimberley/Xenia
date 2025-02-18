﻿using ErrorOr;

namespace Common.Application;

public static class DatabaseErrors
{
    public static Error MaximumRetryError { get; } = Error.Unexpected(
        code: "Database.MaximumRetryReached",
        description: "Maximum number of retries to the Database has been reached");
}