﻿namespace ScreenMedia.Xenia.WebApi.Utilities;

public readonly struct Result<TValue, TError>
{
    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    private readonly TValue? _value;
    private readonly TError? _error;

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private Result(TError error)
    {
        IsError = true;
        _error = error;
        _value = default;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure) =>
        !IsError ? success(_value!) : failure(_error!);
}
