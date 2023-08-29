﻿using Microsoft.EntityFrameworkCore;
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
    public DbSet<Room> Rooms { get; set; }

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
            _ = builder.HasMany(e => e.Bookings)
                       .WithOne()
                       .HasForeignKey("RoomId");
        }
    }
}
