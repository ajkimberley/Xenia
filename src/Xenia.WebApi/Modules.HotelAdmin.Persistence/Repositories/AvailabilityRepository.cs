using Common.Persistence;

using Microsoft.EntityFrameworkCore;

using Modules.HotelAdmin.Domain.Availabilities;

namespace Modules.HotelAdmin.Persistence.Repositories;

public class AvailabilityRepository(HotelAdminContext context) : GenericRepository<Availability, HotelAdminContext>(context), IAvailabilityRepository
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
}