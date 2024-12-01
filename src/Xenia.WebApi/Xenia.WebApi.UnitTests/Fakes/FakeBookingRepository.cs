using Xenia.Bookings.Domain.Bookings;

namespace Xenia.WebApi.Commands.UnitTests.Fakes;
internal class FakeBookingRepository : FakeGenericRepository<Booking>, IBookingRepository
{
    public Task<IEnumerable<Booking>> GetAllAsync(string? bookingRef)
        => throw new NotImplementedException();

    public override Task AddAsync(Booking entity)
    {
        List.Add(entity);
        entity.RoomType?.AddBooking(entity);
        return Task.CompletedTask;
    }
}
