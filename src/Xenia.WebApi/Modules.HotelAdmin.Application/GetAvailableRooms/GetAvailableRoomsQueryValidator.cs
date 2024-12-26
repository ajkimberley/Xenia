using FluentValidation;

namespace Modules.HotelAdmin.Application.GetAvailableRooms;

public class GetAvailableRoomsQueryValidator : AbstractValidator<GetAvailableRoomsQuery>
{
    public GetAvailableRoomsQueryValidator() => _ = RuleFor(query => query.From).LessThan(query => query.To);
}
