using FluentValidation;

using Xenia.WebApi.Queries;

namespace Xenia.WebApi.Validation;

public class GetAvailableRoomsQueryValidator : AbstractValidator<GetAvailableRoomsQuery>
{
    public GetAvailableRoomsQueryValidator()
    {
        _ = RuleFor(query => query.From).NotNull().When(query => query.To.HasValue);
        _ = RuleFor(query => query.To).NotNull().When(query => query.From.HasValue);
        _ = RuleFor(query => query.From).LessThan(query => query.To);
    }
}
