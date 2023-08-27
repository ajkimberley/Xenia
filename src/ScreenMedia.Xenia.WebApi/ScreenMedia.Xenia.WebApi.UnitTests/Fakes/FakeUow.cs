using ScreenMedia.Xenia.HotelManagement.Domain;
using ScreenMedia.Xenia.HotelManagement.Domain.Repositories;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeUow : IUnitOfWork
{
    public FakeUow() => Hotels = new FakeHotelRepository();

    public IHotelRepository Hotels { get; private set; }

    public Task<int> CompleteAsync() => Task.FromResult(1);
    public void Dispose() { }
}
