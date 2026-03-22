using FiapCloudGamesCatalog.Domain.Aggregates;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class Cart : AggregateRoot
{
    public required Guid UserId { get; set; }
    public required bool Active { get; set; }
    public IList<Game> Games { get; set; } = [];

    [SetsRequiredMembers]
    public Cart(Guid userId, IList<Game> games) : base()
    {
        UserId = userId;
        Games = games;
        Active = true;

        AddDomainEvent(new CartCreatedDomainEvent(Id, UserId));
    }

    [SetsRequiredMembers]
    protected Cart()
    {
    }

    public void AddGame(Game game)
    {
        if (Games.Any(g => g.Id == game.Id))
            throw new InvalidOperationAddingGameToCartException();

        Games.Add(game);
        DateUpdated = DateTime.UtcNow;
        AddDomainEvent(new CartGameAddedDomainEvent(Id, UserId, game.Id));
    }

    public void RemoveGame(Game game)
    {
        var gameToRemove = Games.FirstOrDefault(g => g.Id == game.Id);
        if (gameToRemove != null)
        {
            Games.Remove(gameToRemove);
            AddDomainEvent(new CartGameRemovedDomainEvent(Id, UserId, game.Id));
        }
        DateUpdated = DateTime.UtcNow;
    }

    public void ClearCart()
    {
        if (Games.Any())
            AddDomainEvent(new CartClearedDomainEvent(Id, UserId));

        Games.Clear();
        DateUpdated = DateTime.UtcNow;
    }

    public void Inactive()
    {
        Active = false;
        DateUpdated = DateTime.UtcNow;
        AddDomainEvent(new CartInactivatedDomainEvent(Id, UserId));
    }
}
