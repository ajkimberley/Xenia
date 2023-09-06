using MediatR;

using ScreenMedia.Xenia.WebApi.Utilities;

namespace ScreenMedia.Xenia.WebApi.Validation;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull => config.AddBehavior<IPipelineBehavior<TRequest, Result<TResponse, ValidationFailed>>, ValidationBehavior<TRequest, TResponse>>();
}