using Xenia.Bookings.Domain;
using Xenia.Bookings.Domain.Availabilities;
using Xenia.Bookings.Domain.Bookings;
using Xenia.Bookings.Domain.Hotels;

namespace Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeUow : IUnitOfWork
{
    public FakeUow()
    {
        Hotels = new FakeHotelRepository();
        Bookings = new FakeBookingRepository();
        Availability = new FakeAvailabilityRepository();
    }

    public IHotelRepository Hotels { get; private set; }

    public IBookingRepository Bookings { get; private set; }
    
    public IAvailabilityRepository Availability { get; private set; }

    public Task<int> CompleteAsync(CancellationToken cancellationToken) => Task.FromResult(1);
}
