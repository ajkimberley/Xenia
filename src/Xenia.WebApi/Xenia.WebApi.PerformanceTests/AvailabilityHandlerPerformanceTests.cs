using BenchmarkDotNet.Attributes;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Xenia.Bookings.Domain.Availabilities;
using Xenia.Bookings.Persistence;

namespace Xenia.WebApi.PerformanceTests;

[MemoryDiagnoser]
public class AvailabilityHandlerPerformanceTests
{
    private BookingContext _dbContext = null!;

    [IterationSetup]
    public void Setup()
    {
        var provider = new ServiceCollection()
            .AddDbContext<BookingContext>(options =>
                options.UseSqlServer("Server=localhost;Database=Xenia;Trusted_Connection=True;TrustServerCertificate=True"))
            .BuildServiceProvider();
    
        var scope = provider.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<BookingContext>();
    }

    [Benchmark]
    public async Task UpsertAvailability_BatchRecords_StandardUpdate()
    {
        var entity = new Availability { AvailableRooms = 10, Date = new DateTime(2024, 1, 1), HotelId = Guid.NewGuid(), RoomType = "Single" };
        
        var existingEntity = await _dbContext.Availabilities
            .FirstOrDefaultAsync(a => 
                a.HotelId == entity.HotelId && 
                a.RoomType == entity.RoomType && 
                a.Date == entity.Date);
        
        if (existingEntity == null) await _dbContext.Availabilities.AddAsync(entity);
        else
        {
            existingEntity.AvailableRooms = entity.AvailableRooms;
            _dbContext.Entry(existingEntity).State = EntityState.Modified;
        }
        
        await _dbContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task UpsertAvailability_BatchRecords_BulkUpsert() => 
        await _dbContext.BulkInsertOrUpdateAsync(new[] { new Availability { AvailableRooms = 10, Date = new DateTime(2024, 1, 1), HotelId = Guid.NewGuid(), RoomType = "Single" } });
}