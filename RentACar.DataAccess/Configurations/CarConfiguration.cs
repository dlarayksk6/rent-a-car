using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.CarId);
            builder.Property(c => c.Brand).HasMaxLength(100);
            builder.Property(c => c.Model).HasMaxLength(100);
            builder.Property(c => c.Color).HasMaxLength(50);
            builder.Property(c => c.Category).HasMaxLength(10);
            builder.Property(c => c.PlateNumber).HasMaxLength(20);
            builder.Property(c => c.ImageUrl).HasMaxLength(500);
            builder.Property(c => c.Description).HasMaxLength(2000);
            builder.Property(c => c.Transmission).HasMaxLength(50);
            builder.Property(c => c.FuelType).HasMaxLength(50);
            builder.Property(c => c.BodyType).HasMaxLength(50);
            builder.Property(c => c.DriveType).HasMaxLength(50);
            builder.Property(c => c.DailyPrice).HasColumnType("decimal(18,2)");
            builder.Property(c => c.ExtraKmPrice).HasColumnType("decimal(18,2)");
            builder.Property(c => c.DepositAmount).HasColumnType("decimal(18,2)");
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasMany(c => c.Reservations)
                   .WithOne(r => r.Car)
                   .HasForeignKey(r => r.CarId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}