using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

using Xenia.Bookings.Domain.Availabilities;

namespace Xenia.Bookings.Persistence.Repositories;

public class AvailabilityRepository(BookingContext context) : GenericRepository<Availability>(context), IAvailabilityRepository
{
    public override async Task AddAsync(Availability entity)
    {
        var existingEntity = await Context.Availabilities
            .FirstOrDefaultAsync(a => 
                a.HotelId == entity.HotelId && 
                a.RoomType == entity.RoomType && 
                a.Date == entity.Date);
    
        if (existingEntity == null) await Context.Availabilities.AddAsync(entity);
        else
        {
            existingEntity.AvailableRooms = entity.AvailableRooms;
            Context.Entry(existingEntity).State = EntityState.Modified;
        }
    }

    public async Task BulkInsertAsync(Availability entity) 
        => await Context.BulkInsertOrUpdateAsync(new[] { entity });
}