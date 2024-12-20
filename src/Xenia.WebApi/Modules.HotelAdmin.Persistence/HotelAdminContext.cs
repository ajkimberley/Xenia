using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modules.HotelAdmin.Domain.Availabilities;
using Modules.HotelAdmin.Domain.Hotels;

namespace Modules.HotelAdmin.Persistence;

public class HotelAdminContext(DbContextOptions<HotelAdminContext> context) : DbContext(context)
{
    public DbSet<Hotel> Hotels { get; init; } = null!;
    public DbSet<RoomType> Rooms { get; init; } = null!;
    public DbSet<Availability> Availabilities { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new HotelEntityTypeConfiguration().Configure(builder.Entity<Hotel>());
        new RoomEntityTypeConfiguration().Configure(builder.Entity<RoomType>());
        new AvailabilityEntityTypeConfiguration().Configure(builder.Entity<Availability>());
        
        _ = builder.ApplyConfiguration(new HotelEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new RoomEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new AvailabilityEntityTypeConfiguration());
    }
    
    private class HotelEntityTypeConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.Name);
            _ = builder.HasMany(e => e.Rooms)
                .WithOne(e => e.Hotel)
                .HasForeignKey("HotelId")
                .IsRequired();
        }
    }
    
    private class RoomEntityTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.Name);
            _ = builder.Property<DateTime>("RowVersion").IsConcurrencyToken();
            // _ = builder.HasMany(r => r.Bookings)
            //     .WithOne(b => b.RoomType)
            //     .HasForeignKey("RoomTypeId")
            //     .IsRequired();
        }
    }
    
    private class AvailabilityEntityTypeConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            _ = builder.HasIndex([nameof(Availability.HotelId), nameof(Availability.RoomType), nameof(Availability.Date)]);
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
            _ = builder.Property(e => e.RoomType).IsRequired();
            _ = builder.Property(e => e.Date).IsRequired();
            _ = builder.Property(e => e.HotelId).IsRequired();
            _ = builder.Property(e => e.AvailableRooms).IsRequired();
        }
    }
}