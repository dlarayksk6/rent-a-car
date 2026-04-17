using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.HasKey(c => c.CampaignId);
            builder.Property(c => c.Title).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.ImageUrl).HasMaxLength(500);
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }
    }
}