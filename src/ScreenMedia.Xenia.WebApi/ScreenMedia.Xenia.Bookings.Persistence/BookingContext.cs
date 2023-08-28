using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScreenMedia.Xenia.Bookings.Domain.Entities;

namespace ScreenMedia.Xenia.Bookings.Persistence;

public class BookingContext : DbContext
{
    public BookingContext(DbContextOptions<BookingContext> context) : base(context)
    {
    }

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) =>
    _ = builder.ApplyConfiguration(new BookingEntityTypeConfiguration());

    private class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder) => _ = builder.HasKey(e => e.Id);
    }
}
