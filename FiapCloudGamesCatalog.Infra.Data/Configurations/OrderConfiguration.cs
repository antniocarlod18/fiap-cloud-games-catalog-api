using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(o => o.Id);
            builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");

            builder.Property(o => o.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.HasMany(o => o.Games)
                .WithOne(o => o.Order);
        }
    }
}
