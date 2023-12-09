using ScreenMedia.Xenia.Bookings.Domain;
using ScreenMedia.Xenia.Bookings.Domain.Repositories;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeUow : IUnitOfWork
{
    public FakeUow()
    {
        Hotels = new FakeHotelRepository();
        Bookings = new FakeBookingRepository();
    }

    public IHotelRepository Hotels { get; private set; }

    public IBookingRepository Bookings { get; private set; }

    public Task<int> CompleteAsync(CancellationToken cancellationToken) => Task.FromResult(1);

    public void Dispose() { }
}
