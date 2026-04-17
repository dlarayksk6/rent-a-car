using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasKey(b => b.BlogPostId);
            builder.Property(b => b.Title).HasMaxLength(300).IsRequired();
            builder.Property(b => b.Content).IsRequired();
            builder.Property(b => b.ImageUrl).HasMaxLength(500);
            builder.Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne<Microsoft.AspNetCore.Identity.IdentityUser>()
                   .WithMany()
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}