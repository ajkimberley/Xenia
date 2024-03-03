using ErrorOr;

using MediatR;

namespace Xenia.WebApi.Validation;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config)
        where TRequest : IRequest<TResponse> 
        where TResponse : IErrorOr =>
        config.AddBehavior<IPipelineBehavior<TRequest, TResponse>, ValidationBehavior<TRequest, TResponse>>();
}