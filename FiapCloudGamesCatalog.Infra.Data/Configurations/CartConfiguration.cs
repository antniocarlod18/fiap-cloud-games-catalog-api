using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Cart");
        builder.HasKey(u => u.Id);
        builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");

        builder.Property(u => u.UserId)
            .IsRequired();

        builder.Property(u => u.Active).IsRequired();

        builder.HasMany(p => p.Games)
            .WithMany(g => g.Cart);
    }
}
