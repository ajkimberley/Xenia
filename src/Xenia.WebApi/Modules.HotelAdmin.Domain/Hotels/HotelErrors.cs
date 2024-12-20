using ErrorOr;

namespace Modules.HotelAdmin.Domain.Hotels;

public static class HotelErrors
{
    public static Error NoVacancyAvailable { get; } = Error.Conflict(
        code: "Hotel.NoVacancy",
        description: "No vacancies are available for the selected room type and dates."
    );
}