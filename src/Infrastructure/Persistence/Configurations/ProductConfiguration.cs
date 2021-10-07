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

            builder.HasMany(p => p.OrderDetails).WithOne(od => od.Product).HasForeignKey(od => od.ProductId);
        }
    }
}