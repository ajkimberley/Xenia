using FluentValidation;

using Xenia.WebApi.RequestsResponses;

namespace Xenia.WebApi.Validation;

public class GetHotelsRequestValidator : AbstractValidator<GetHotelsRequest>
{
    public GetHotelsRequestValidator() =>
        RuleFor(query => query.Name).MinimumLength(3).WithMessage("Hotel name must be at least three characters long.");
}