using Xenia.Bookings.Domain.Availabilities;

namespace Xenia.WebApi.Commands.UnitTests.Fakes;

internal class FakeAvailabilityRepository : FakeGenericRepository<Availability>, IAvailabilityRepository
{
    public Task<IEnumerable<Availability>> GetAllAsync(string? bookingRef)
        => throw new NotImplementedException();

    public override Task AddAsync(Availability entity)
    {
        List.Add(entity);
        return Task.CompletedTask;
    }
}