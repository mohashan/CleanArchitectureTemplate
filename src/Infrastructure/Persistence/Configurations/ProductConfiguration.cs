using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.Description).IsRequired(false);

            builder.Property(p => p.CreatedBy).IsRequired().HasMaxLength(10);
            builder.Property(p => p.Created).IsRequired();
            builder.Property(p => p.LastModifiedBy).IsRequired(false).HasMaxLength(10);
            builder.Property(p => p.LastModified).IsRequired(false);
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasMany(p => p.OrderDetails).WithOne(od => od.Product).HasForeignKey(od => od.ProductId);
        }
    }
}