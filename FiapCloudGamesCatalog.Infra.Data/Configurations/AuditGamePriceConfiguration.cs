using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations
{
    public class AuditGamePriceConfiguration : IEntityTypeConfiguration<AuditGamePrice>
    {
        public void Configure(EntityTypeBuilder<AuditGamePrice> builder)
        {
            builder.ToTable("AuditGamePrice");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.DateCreated).HasColumnType("DATETIME").IsRequired();

            builder.Property(o => o.Justification).HasMaxLength(300);

            builder.Property(o => o.OldPrice)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.NewPrice)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.HasOne(o => o.Game)
                .WithMany()
                .HasForeignKey("GameId")
                .IsRequired();

            builder.Ignore(p => p.DateUpdated);
        }
    }
}
