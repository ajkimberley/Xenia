using FluentValidation;

using Xenia.Application.Bookings;

namespace Xenia.WebApi.Validation;

public class CreateBookingRequestValidator :  AbstractValidator<BookingDto>
{
    public CreateBookingRequestValidator() =>
        RuleFor(x => x.From)
            .LessThan(x => x.To)
            .WithMessage("From Date must be earlier than To Date.");
}