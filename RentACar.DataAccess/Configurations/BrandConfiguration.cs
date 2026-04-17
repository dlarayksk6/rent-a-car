using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(b => b.BrandId);
            builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
            builder.Property(b => b.LogoUrl).HasMaxLength(500);
            builder.Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }
    }
}