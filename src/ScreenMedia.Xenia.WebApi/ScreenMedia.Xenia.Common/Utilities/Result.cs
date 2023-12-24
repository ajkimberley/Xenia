using System.ComponentModel;

namespace ScreenMedia.Xenia.Common.Utilities;

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
    
    public async Task<TResult> Match<TResult>(Func<TValue, Task<TResult>> success, Func<TError, Task<TResult>> failure) 
        => !IsError ? await success(_value!) : await failure(_error!);
    
    public async Task<TResult> Match<TResult>(Func<TValue, Task<TResult>> success, Func<TError, TResult> failure) 
        => !IsError ? await success(_value!) : failure(_error!);
    
    public async Task<TResult> Match<TResult>(Func<TValue, TResult> success, Func<TError, Task<TResult>> failure) 
        => !IsError ? success(_value!) : await failure(_error!);
    
    public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> f) => 
        IsError ? _error! : f(_value!);
    
    public async Task<Result<TResult, TError>> Map<TResult>(Func<TValue, Task<TResult>> f) => 
        IsError ? _error! : await f(_value!);
    
    public Result<TResult, TError> Map2<TResult, TValue2>(Func<TValue, TValue2, TResult> f, TValue2 value2) => 
        IsError ? _error! : f(_value!, value2);
    
    public async Task<Result<TResult, TError>> Map2<TResult, TValue2>(Func<TValue, TValue2, Task<TResult>> f, TValue2 value2) => 
        IsError ? _error! : await f(_value!, value2);
}