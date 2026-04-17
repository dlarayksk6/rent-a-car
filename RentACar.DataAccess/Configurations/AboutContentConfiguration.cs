using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class AboutContentConfiguration : IEntityTypeConfiguration<AboutContent>
    {
        public void Configure(EntityTypeBuilder<AboutContent> builder)
        {
            builder.HasKey(a => a.AboutContentId);
            builder.Property(a => a.Title).HasMaxLength(200);
            builder.Property(a => a.SubTitle).HasMaxLength(300);
            builder.Property(a => a.ImageUrl).HasMaxLength(500);
            builder.Property(a => a.BannerTitle).HasMaxLength(300);
            builder.Property(a => a.HeroTitle).HasMaxLength(200);
            builder.Property(a => a.HeroSubText).HasMaxLength(500);
            builder.Property(a => a.Feature1Title).HasMaxLength(100);
            builder.Property(a => a.Feature2Title).HasMaxLength(100);
            builder.Property(a => a.Feature3Title).HasMaxLength(100);
            builder.Property(a => a.Feature4Title).HasMaxLength(100);
        }
    }
}