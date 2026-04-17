using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Entities;

namespace RentACar.DataAccess.Configurations
{
    public class ContractDocumentConfiguration : IEntityTypeConfiguration<ContractDocument>
    {
        public void Configure(EntityTypeBuilder<ContractDocument> builder)
        {
            builder.HasKey(c => c.ContractId);
            builder.Property(c => c.Title).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.PdfUrl).HasMaxLength(500);
        }
    }
}