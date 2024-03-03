using ErrorOr;

namespace Xenia.Bookings.Domain.Errors;

public static class HotelErrors
{
    public static Error NoVacancyAvailable = Error.Conflict(
        code: "Hotel.NoVacancy",
        description: "No vacancies are available for the selected room type and dates."
    );
}