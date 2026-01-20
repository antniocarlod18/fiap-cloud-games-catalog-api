using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class OrderGameItem : EntityBase
{
    public required Order Order { get; set; }
    public required Game Game { get; set; }
    public required decimal Price { get; set; }

    [SetsRequiredMembers]
    public OrderGameItem(Order order, Game game) : base()
    {
        Order = order;
        Game = game;
        Price = game.GetPriceWithDiscount();
    }

    [SetsRequiredMembers]
    public OrderGameItem()
    {
    }
}
