using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotion");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");

            builder.Property(p => p.DiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(p => p.StartDate).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.EndDate).HasColumnType("DATETIME").IsRequired();

            // Computed property, don't map to database
            builder.Ignore(p => p.Active);

            builder.HasMany(p => p.Games)
                .WithMany(g => g.Promotions);
        }
    }
}
