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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderGameItem>
    {
        public void Configure(EntityTypeBuilder<OrderGameItem> builder)
        {
            builder.ToTable("OrderGameItem");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.HasOne(p => p.Order)
                .WithMany(g => g.Games);

            builder.HasOne(p => p.Game)
                .WithMany();

            builder.Ignore(p => p.DateUpdated);
        }
    }
}
