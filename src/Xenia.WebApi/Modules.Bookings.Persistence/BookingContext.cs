using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Modules.Bookings.Domain;

namespace Modules.Bookings.Persistence;

public class BookingContext(DbContextOptions<BookingContext> context) : DbContext(context)
{ 
    public DbSet<Booking> Bookings { get; init; } = null!;
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        new BookingEntityTypeConfiguration().Configure(builder.Entity<Booking>());

        _ = builder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        
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
