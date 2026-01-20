using FiapCloudGamesCatalog.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class Game : EntityBase
{
    public required string Title { get; set; }
    public required string Genre { get; set; } 
    public string? Description { get; set; }
    public required decimal Price { get ; set; }
    public string? Developer { get; set; }
    public string? Distributor { get; set; }
    public required IList<GamePlatformEnum> GamePlatforms { get; set; }
    public string? GameVersion { get; set; }
    public bool Available { get; set; }
    public IList<Promotion>? Promotions { get; set; } = [];
    public IList<Library> Library { get; set; } = [];
    public IList<Cart> Cart { get; set; } = [];
    
    [SetsRequiredMembers]
    public Game(string title, string genre, IList<GamePlatformEnum> gamePlatforms, string? description, decimal price, string? developer, 
        string? distributor,  string? gameVersion, bool available) : base()
    {
        this.Title = title;
        this.Genre = genre;
        this.Description = description;
        this.Price = price;
        this.Developer = developer;
        this.Distributor = distributor;
        this.GamePlatforms = gamePlatforms;
        this.GameVersion = gameVersion;
        this.Available = available;
    }

    [SetsRequiredMembers]
    protected Game()
    {
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
        DateUpdated = DateTime.UtcNow;
    }

    public decimal GetPriceWithDiscount()
    {
        if (Promotions == null || !Promotions.Any(p => p.Active))
        {
            return Price;
        }

        var maxDiscount = Promotions
            .Where(p => p.Active)
            .Max(p => p.DiscountPercentage);
        var discountedPrice = Price - (Price * (maxDiscount / 100));
        
        return Math.Round(discountedPrice, 2);
    }  
}
