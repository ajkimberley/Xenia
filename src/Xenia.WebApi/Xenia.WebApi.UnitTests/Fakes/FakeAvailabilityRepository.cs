using Xenia.Bookings.Domain.Availabilities;

namespace Xenia.WebApi.Commands.UnitTests.Fakes;

internal class FakeAvailabilityRepository : FakeGenericRepository<Availability>, IAvailabilityRepository
{
    public Task<IEnumerable<Availability>> GetAllAsync(string? bookingRef)
        => throw new NotImplementedException();

    public override Task AddAsync(Availability entity)
    {
        var existingEntity = List
            .FirstOrDefault(a => 
                a.HotelId == entity.HotelId && 
                a.RoomType == entity.RoomType && 
                a.Date == entity.Date);
    
        if (existingEntity == null) List.Add(entity);
        else existingEntity.AvailableRooms = entity.AvailableRooms;
        return Task.CompletedTask;
    }
}