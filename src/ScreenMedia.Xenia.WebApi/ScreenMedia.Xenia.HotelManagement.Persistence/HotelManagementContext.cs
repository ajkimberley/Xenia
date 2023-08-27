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

    protected override void OnModelCreating(ModelBuilder builder) => _ = builder.ApplyConfiguration(new HotelEntityTypeConfiguration());

    private class HotelEntityTypeConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            _ = builder.HasKey(e => e.Id);
            _ = builder.Property(e => e.Name);
        }
    }
}
