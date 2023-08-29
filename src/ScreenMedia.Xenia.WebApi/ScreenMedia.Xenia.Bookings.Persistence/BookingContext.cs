using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScreenMedia.Xenia.Bookings.Domain.Entities;

namespace ScreenMedia.Xenia.Bookings.Persistence;

public class BookingContext : DbContext
{
    public BookingContext(DbContextOptions<BookingContext> context) : base(context)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        _ = builder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new HotelEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new RoomEntityTypeConfiguration());
    }

    private class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder) => _ = builder.HasKey(e => e.Id);
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
    private class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.Number);
            _ = builder.Property(e => e.Type).HasConversion<int>();
        }
    }
}
