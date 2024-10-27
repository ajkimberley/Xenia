﻿using ErrorOr;

namespace Xenia.Common.Errors;

public static class RestErrors
{
    public static Error ResourceNotFoundError { get; } = Error.Unexpected(
        code: "RestError.ResourceNotFound",
        description: "The requested resource cannot be found.");
}