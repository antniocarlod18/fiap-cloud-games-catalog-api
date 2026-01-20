using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations;

public class AuditGameUserCollectionConfiguration : IEntityTypeConfiguration<AuditGameUserCollection>
{
    public void Configure(EntityTypeBuilder<AuditGameUserCollection> builder)
    {
        builder.ToTable("AuditGameUserCollection");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.DateCreated).HasColumnType("DATETIME").IsRequired();

        builder.Property(o => o.Notes).HasMaxLength(300);

        builder.Property(o => o.Action)
            .IsRequired();

        builder.Property(o => o.Collection)
            .IsRequired();

        builder.Property(o => o.UserId)
            .IsRequired();

        builder.HasOne(o => o.Game)
            .WithMany()
            .HasForeignKey("GameId")
            .IsRequired();

        builder.Ignore(p => p.DateUpdated);
    }
}
