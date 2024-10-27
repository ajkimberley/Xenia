using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Xenia.Bookings.Domain.Entities;

namespace Xenia.Bookings.Persistence;

public class BookingContext(DbContextOptions<BookingContext> context) : DbContext(context)
{
    public required DbSet<Hotel> Hotels { get; set; }
    public required DbSet<Booking> Bookings { get; set; }
    public required DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new BookingEntityTypeConfiguration().Configure(builder.Entity<Booking>());
        new HotelEntityTypeConfiguration().Configure(builder.Entity<Hotel>());
        new RoomEntityTypeConfiguration().Configure(builder.Entity<Room>());

        _ = builder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new HotelEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new RoomEntityTypeConfiguration());
    }

    private class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.RoomType).IsRequired();
            _ = builder.Property(e => e.BookerName).IsRequired();
            _ = builder.Property(e => e.BookerEmail).IsRequired();
            _ = builder.Property(e => e.State).IsRequired();
            _ = builder.Property(e => e.From).IsRequired();
            _ = builder.Property(e => e.To).IsRequired();
            _ = builder.HasIndex(e => e.Reference).IsUnique();
        }
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
            _ = builder.Property<DateTime>("RowVersion").IsConcurrencyToken();
            _ = builder.HasMany(r => r.Bookings)
                       .WithOne(b => b.Room)
                       .HasForeignKey("RoomId")
                       .IsRequired();
        }
    }
}
