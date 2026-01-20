using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class AuditGamePrice : EntityBase
{
    public Game Game { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public string? Justification { get; set; }

    [SetsRequiredMembers]
    public AuditGamePrice(Game game, decimal oldPrice, decimal newPrice, string? justification = null) : base()
    {
        Game = game;
        OldPrice = oldPrice;
        NewPrice = newPrice;
        Justification = justification;
    }

    [SetsRequiredMembers]
    protected AuditGamePrice()
    {
    }
}
