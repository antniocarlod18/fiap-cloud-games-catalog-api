using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesCatalog.Infra.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Game");
        builder.HasKey(g => g.Id);
        builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");
        builder.Property(g => g.Title).IsRequired().HasMaxLength(200);
        builder.Property(g => g.Genre).IsRequired().HasMaxLength(100);
        builder.Property(g => g.Description).HasMaxLength(1000);
        builder.Property(g => g.Price).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(g => g.Developer).HasMaxLength(100);
        builder.Property(g => g.Distributor).HasMaxLength(100);
        builder.Property(g => g.GameVersion).HasMaxLength(50);
        builder.Property(g => g.Available).IsRequired(true);
        builder.Property(g => g.GamePlatforms) 
            .HasConversion(
                v => string.Join(",", v.Select(e => e.ToString())), 
                v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => (GamePlatformEnum)System.Enum.Parse(typeof(GamePlatformEnum), s))
                        .ToList()
            );
        // You might also need a ValueComparer for collections to ensure proper change tracking
        var valueComparer = new ValueComparer<IList<GamePlatformEnum>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => System.HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        builder
            .Property(e => e.GamePlatforms)
            .Metadata.SetValueComparer(valueComparer);

        builder.HasMany(g => g.Promotions)
            .WithMany(p => p.Games);
        builder.HasMany(g => g.Library)
            .WithMany(u => u.Games)
            .UsingEntity("GamesLibrary"); 
        builder.HasMany(g => g.Cart)
            .WithMany(u => u.Games)
            .UsingEntity("GamesCart"); 
    }
}
