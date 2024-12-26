using FluentValidation;

using Modules.Bookings.Application;

namespace Modules.HotelAdmin.Endpoints.Validators;

public class CreateBookingRequestValidator :  AbstractValidator<BookingDto>
{
    public CreateBookingRequestValidator() =>
        RuleFor(x => x.From)
            .LessThan(x => x.To)
            .WithMessage("From Date must be earlier than To Date.");
}