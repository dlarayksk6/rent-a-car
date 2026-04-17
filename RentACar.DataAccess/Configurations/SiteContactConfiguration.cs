using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class SiteContactConfiguration : IEntityTypeConfiguration<SiteContact>
    {
        public void Configure(EntityTypeBuilder<SiteContact> builder)
        {
            builder.HasKey(s => s.SiteContactId);
            builder.Property(s => s.Phone).HasMaxLength(20);
            builder.Property(s => s.Email).HasMaxLength(150);
            builder.Property(s => s.Address).HasMaxLength(300);
            builder.Property(s => s.Facebook).HasMaxLength(300);
            builder.Property(s => s.Instagram).HasMaxLength(300);
            builder.Property(s => s.Twitter).HasMaxLength(300);
            builder.Property(s => s.WhatsappNumber).HasMaxLength(20);
            builder.Property(s => s.WorkingHours).HasMaxLength(100);
            builder.Property(s => s.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }
    }
}