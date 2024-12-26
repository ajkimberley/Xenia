using FluentValidation;

using Modules.HotelAdmin.Endpoints.RequestResponse;

namespace Modules.HotelAdmin.Endpoints.Validators;

public class GetHotelsRequestValidator : AbstractValidator<GetHotelsRequest>
{
    public GetHotelsRequestValidator() =>
        RuleFor(query => query.Name).MinimumLength(3).WithMessage("Hotel name must be at least three characters long.");
}