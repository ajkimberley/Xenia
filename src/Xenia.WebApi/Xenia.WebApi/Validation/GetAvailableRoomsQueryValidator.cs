using FluentValidation;

using Xenia.Application.Queries;

namespace Xenia.WebApi.Validation;

public class GetAvailableRoomsQueryValidator : AbstractValidator<GetAvailableRoomsQuery>
{
    public GetAvailableRoomsQueryValidator()
    {
        _ = RuleFor(query => query.From).LessThan(query => query.To);
    }
}
