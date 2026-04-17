using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.ReservationId);
            builder.Property(r => r.Status).HasMaxLength(50).IsRequired();
            builder.Property(r => r.PickupLocation).HasMaxLength(200).IsRequired();
            builder.Property(r => r.DropoffLocation).HasMaxLength(200).IsRequired();
            builder.Property(r => r.FullName).HasMaxLength(150).IsRequired();
            builder.Property(r => r.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(r => r.NationalId).HasMaxLength(11).IsRequired();
            builder.Property(r => r.Notes).HasMaxLength(500);
            builder.Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        }
    }
}