using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Xenia.Bookings.Domain.Entities;
using Xenia.Bookings.Domain.Enums;

namespace Xenia.Bookings.Persistence;

public class BookingContext(DbContextOptions<BookingContext> context) : DbContext(context)
{
    public required DbSet<Hotel> Hotels { get; init; }
    public required DbSet<Booking> Bookings { get; init; }
    public required DbSet<RoomType> Rooms { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new BookingEntityTypeConfiguration().Configure(builder.Entity<Booking>());
        new HotelEntityTypeConfiguration().Configure(builder.Entity<Hotel>());
        new RoomEntityTypeConfiguration().Configure(builder.Entity<RoomType>());

        _ = builder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new HotelEntityTypeConfiguration());
        _ = builder.ApplyConfiguration(new RoomEntityTypeConfiguration());
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
            _ = builder.HasMany(r => r.Bookings)
                       .WithOne(b => b.RoomType)
                       .HasForeignKey("RoomTypeId")
                       .IsRequired();
        }
    }
    
    private class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.BookerName).IsRequired();
            _ = builder.Property(e => e.BookerEmail).IsRequired();
            _ = builder.Property(e => e.State).IsRequired();
            _ = builder.Property(e => e.From).IsRequired();
            _ = builder.Property(e => e.To).IsRequired();
            _ = builder.HasIndex(e => e.Reference).IsUnique();
        }
    }
}
