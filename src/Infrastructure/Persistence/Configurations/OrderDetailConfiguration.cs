using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.Id);

            builder.Property(od => od.OrderId).IsRequired();
            builder.Property(od => od.ProductId).IsRequired();
            builder.Property(od => od.Amount).IsRequired();
            builder.Property(od => od.Count).IsRequired();

            builder.Property(od => od.CreatedBy).IsRequired().HasMaxLength(10);
            builder.Property(od => od.Created).IsRequired();
            builder.Property(od => od.LastModifiedBy).IsRequired(false).HasMaxLength(10);
            builder.Property(od => od.LastModified).IsRequired(false);
            builder.Property(od => od.IsDeleted).IsRequired();

            builder.HasOne(od => od.Product).WithMany(p => p.OrderDetails).HasForeignKey(od => od.ProductId);
            builder.HasOne(od => od.Order).WithMany(o => o.OrderDetails).HasForeignKey(od => od.OrderId);
        }
    }
}