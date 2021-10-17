using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Amount).IsRequired();
            builder.Property(o => o.UserName).IsRequired().HasMaxLength(100);

            builder.Property(o => o.CreatedBy).IsRequired().HasMaxLength(10);
            builder.Property(o => o.Created).IsRequired();
            builder.Property(o => o.LastModifiedBy).IsRequired(false).HasMaxLength(10);
            builder.Property(o => o.LastModified).IsRequired(false);
            builder.Property(o => o.IsDeleted).IsRequired();


            builder.HasMany(o => o.OrderDetails).WithOne(od => od.Order).HasForeignKey(od => od.OrderId);
        }
    }
}