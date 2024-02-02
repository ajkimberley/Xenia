using MediatR;

using Xenia.Common.Utilities;

namespace Xenia.WebApi.Validation;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull => config.AddBehavior<IPipelineBehavior<TRequest, Result<TResponse, ValidationFailed>>, ValidationBehavior<TRequest, TResponse>>();
}