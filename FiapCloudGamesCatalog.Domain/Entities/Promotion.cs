using FiapCloudGamesCatalog.Domain.Aggregates;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class Promotion : AggregateRoot
{
    public IList<Game> Games { get; set; } = [];
    public required decimal DiscountPercentage { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public bool Active { 
        get
        {
            return DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
        }
    }

    [SetsRequiredMembers]
    public Promotion(IList<Game> gamePromotions, decimal discountPercentage, DateTime startDate, DateTime endDate) : base()
    {
        Games = gamePromotions;
        DiscountPercentage = discountPercentage;
        StartDate = startDate;
        EndDate = endDate;

        AddDomainEvent(new PromotionCreatedDomainEvent(
            Id,
            DiscountPercentage,
            StartDate,
            EndDate,
            Games.Select(g => g.Id).ToList()));
    }

    [SetsRequiredMembers]
    protected Promotion()
    {
    }

    public void AddGame(Game game)
    {
        if (Games.Any(g => g.Id == game.Id))
            throw new InvalidOperationAddingGameToCartException(); 

        Games.Add(game);
        AddDomainEvent(new PromotionGameAddedDomainEvent(Id, game.Id));
    }

    public void RemoveGame(Game game)
    {
        var gameToRemove = Games.FirstOrDefault(g => g.Id == game.Id);
        if (gameToRemove != null)
        {
            Games.Remove(gameToRemove);
            AddDomainEvent(new PromotionGameRemovedDomainEvent(Id, game.Id));
        }
    }

    public void Delete()
    {
        AddDomainEvent(new PromotionDeletedDomainEvent(Id));
    }
}
