using ScreenMedia.Xenia.Bookings.Domain.Entities;
using ScreenMedia.Xenia.Bookings.Domain.Repositories;

namespace ScreenMedia.Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeBookingRepository : FakeGenericRepository<Booking>, IBookingRepository
{
    public Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef)
        => throw new NotImplementedException();

    public override Task AddAsync(Booking entity)
    {
        _list.Add(entity);
        entity.Room?.AddBooking(entity);
        return Task.CompletedTask;
    }
}
