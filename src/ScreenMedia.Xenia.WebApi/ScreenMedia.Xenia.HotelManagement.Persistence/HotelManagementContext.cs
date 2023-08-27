using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScreenMedia.Xenia.HotelManagement.Domain.Entities;

namespace ScreenMedia.Xenia.HotelManagement.Persistence;

public class HotelManagementContext : DbContext
{
    public HotelManagementContext(DbContextOptions<HotelManagementContext> context) : base(context)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) =>
        _ = builder.ApplyConfiguration(new HotelEntityTypeConfiguration())
                   .ApplyConfiguration(new RoomEntityTypeConfiguration());

    private class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.Number);
            _ = builder.Property(e => e.Type).HasConversion<int>();
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
}
