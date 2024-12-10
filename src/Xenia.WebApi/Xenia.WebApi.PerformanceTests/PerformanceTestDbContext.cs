using Microsoft.EntityFrameworkCore;

using Xenia.Bookings.Persistence;

namespace Xenia.WebApi.PerformanceTests;

public class PerformanceTestDbContext(DbContextOptions<BookingContext> options) : BookingContext(options)
{
    public static BookingContext Create()
    {
        var configuration = PerformanceTestSetup.GetConfiguration();
        var optionsBuilder = new DbContextOptionsBuilder<BookingContext>();
        optionsBuilder.UseInMemoryDatabase("Xenia");

        return new BookingContext(optionsBuilder.Options)
        {
            Hotels = null!,
            Bookings = null!,
            Rooms = null!,
            Availabilities = null!
        };
    }
}