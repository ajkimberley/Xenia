using FluentValidation;

using Xenia.Application.Dtos;

namespace Xenia.WebApi.Validators;

public class CreateBookingRequestValidator :  AbstractValidator<BookingDto>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.From)
            .LessThan(x => x.To);
    }
}