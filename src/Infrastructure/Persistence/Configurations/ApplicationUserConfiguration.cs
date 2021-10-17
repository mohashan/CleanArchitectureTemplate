using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Family).IsRequired().HasMaxLength(50);
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.Property(u => u.NormalizedEmail).IsRequired().HasMaxLength(50);
            builder.Property(u => u.BirthDay).IsRequired(false);
            builder.Property(u => u.Gender).IsRequired();
            builder.Property(u => u.ConcurrencyStamp).IsRequired().HasMaxLength(50);

            builder.Property(u => u.IsDeleted).IsRequired();
            builder.Property(u => u.CreatedBy).IsRequired().HasMaxLength(10);
            builder.Property(u => u.Created).IsRequired();
            builder.Property(u => u.LastModifiedBy).IsRequired(false).HasMaxLength(10);
            builder.Property(u => u.LastModified).IsRequired(false);

        }
    }
}