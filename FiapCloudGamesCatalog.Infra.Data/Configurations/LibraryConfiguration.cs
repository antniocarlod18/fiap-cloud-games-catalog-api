using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.ToTable("Library");
        builder.HasKey(u => u.Id);
        builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");

        builder.Property(u => u.UserId)
            .IsRequired();

        builder.Property(u => u.Active).IsRequired();

        builder.HasMany(p => p.Games)
            .WithMany(g => g.Library);
    }
}
