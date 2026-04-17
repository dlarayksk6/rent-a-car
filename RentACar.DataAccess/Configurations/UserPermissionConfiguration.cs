using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.HasKey(up => up.Id);

            builder.HasOne(up => up.Permission)
                   .WithMany()
                   .HasForeignKey(up => up.PermissionId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}