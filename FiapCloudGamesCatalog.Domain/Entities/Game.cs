using FiapCloudGamesCatalog.Domain.Aggregates;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class Game : AggregateRoot
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

        AddDomainEvent(new GameCreatedDomainEvent(Id, Title, Genre, Price, Available, GamePlatforms.ToList()));
    }

    [SetsRequiredMembers]
    protected Game()
    {
    }

    public void UpdatePrice(decimal newPrice)
    {
        var oldPrice = Price;
        Price = newPrice;
        DateUpdated = DateTime.UtcNow;
        AddDomainEvent(new GamePriceUpdatedDomainEvent(Id, oldPrice, newPrice));
    }

    public void UpdateDetails(
        string title,
        string genre,
        IList<GamePlatformEnum> gamePlatforms,
        string? description,
        string? developer,
        string? distributor,
        string? gameVersion,
        bool available)
    {
        Title = title;
        Genre = genre;
        Description = description;
        Developer = developer;
        Distributor = distributor;
        GamePlatforms = gamePlatforms;
        GameVersion = gameVersion;
        Available = available;
        DateUpdated = DateTime.UtcNow;

        AddDomainEvent(new GameUpdatedDomainEvent(Id, Title, Genre, Available));
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
