using FluentValidation;

using MediatR;

using Xenia.Common.Utilities;

namespace Xenia.WebApi.Validation;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, Result<TResult, ValidationFailed>> where TRequest : notnull
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator) => _validator = validator;

    public async Task<Result<TResult, ValidationFailed>> Handle(
        TRequest request,
        RequestHandlerDelegate<Result<TResult, ValidationFailed>> next,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        return !validationResult.IsValid ? (Result<TResult, ValidationFailed>)new ValidationFailed(validationResult.Errors) : await next();
    }
}
