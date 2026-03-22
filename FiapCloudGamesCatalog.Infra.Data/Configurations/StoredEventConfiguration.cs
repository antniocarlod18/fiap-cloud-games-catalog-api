using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations;

public class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
{
    public void Configure(EntityTypeBuilder<StoredEvent> builder)
    {
        builder.ToTable("StoredEvent");

        builder.HasKey(e => e.Id);

        builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");

        builder.Property(e => e.AggregateId).IsRequired();
        builder.Property(e => e.AggregateType).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Version).IsRequired();
        builder.Property(e => e.EventType).IsRequired().HasMaxLength(500);
        builder.Property(e => e.OccurredOn).HasColumnType("DATETIME").IsRequired();
        builder.Property(e => e.Data).IsRequired().HasColumnType("LONGTEXT");
        builder.Property(e => e.Metadata).HasColumnType("LONGTEXT");

        builder.HasIndex(e => new { e.AggregateId, e.Version }).IsUnique();
        builder.HasIndex(e => e.AggregateId);
    }
}
