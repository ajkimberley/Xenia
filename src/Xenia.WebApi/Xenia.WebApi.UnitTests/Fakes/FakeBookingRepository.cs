using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Repositories;

namespace Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeBookingRepository : FakeGenericRepository<Booking>, IBookingRepository
{
    public Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef)
        => throw new NotImplementedException();

    public override Task AddAsync(Booking entity)
    {
        List.Add(entity);
        entity.Room?.AddBooking(entity);
        return Task.CompletedTask;
    }
}
