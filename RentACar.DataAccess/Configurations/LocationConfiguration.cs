using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.LocationId);
            builder.ToTable("Locations");
            builder.Property(l => l.Name).HasMaxLength(200).IsRequired();
            builder.Property(l => l.Address).HasMaxLength(300);
        }
    }
}