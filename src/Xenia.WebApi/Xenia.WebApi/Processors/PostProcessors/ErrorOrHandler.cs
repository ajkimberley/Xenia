using ErrorOr;

using FastEndpoints;

using FluentValidation.Results;

namespace Xenia.WebApi.Processors.PostProcessors;

// ReSharper disable once ClassNeverInstantiated.Global
sealed class ErrorOrHandler : IGlobalPostProcessor
{
    public Task PostProcessAsync(IPostProcessorContext ctx, CancellationToken ct)
    {
        if (ctx.HttpContext.ResponseStarted() || ctx.Response is not IErrorOr errorOr) return Task.CompletedTask;

        if (!errorOr.IsError) return Task.CompletedTask;


        if (errorOr.Errors?.All(e => e.Type == ErrorType.Validation) is true)
            return ctx.HttpContext.Response.SendErrorsAsync(
                failures: [..errorOr.Errors.Select(e => new ValidationFailure(e.Code, e.Description))],
                cancellation: ct);

        var problem = errorOr.Errors?.FirstOrDefault(e => e.Type != ErrorType.Validation);

        return problem?.Type switch
        {
            ErrorType.Conflict => ctx.HttpContext.Response.SendAsync("Duplicate submission!", 409, cancellation: ct),
            ErrorType.NotFound => ctx.HttpContext.Response.SendNotFoundAsync(ct),
            ErrorType.Unauthorized => ctx.HttpContext.Response.SendUnauthorizedAsync(ct),
            ErrorType.Forbidden => ctx.HttpContext.Response.SendForbiddenAsync(ct),
            null => throw new InvalidOperationException(),
            _ => Task.CompletedTask
        };
    }
}