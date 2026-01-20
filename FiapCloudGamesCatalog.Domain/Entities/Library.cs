using FiapCloudGamesCatalog.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class Library : EntityBase
{
    public required Guid UserId { get; set; }
    public required bool Active { get; set; }
    public IList<Game> Games { get; set; } = [];

    [SetsRequiredMembers]
    public Library(Guid userId, IList<Game> games) : base()
    {
        UserId = userId;
        Games = games;
        Active = true;
    }

    [SetsRequiredMembers]
    protected Library()
    {
    }

    public void Add(Game game)
    {
        if (Games.Any(g => g.Id == game.Id))
            throw new InvalidOperationAddingGameToCartException();

        DateUpdated = DateTime.UtcNow;
        Games.Add(game);
    }

    public void Remove(Game game)
    {
        var gameToRemove = Games.FirstOrDefault(g => g.Id == game.Id);
        if (gameToRemove != null)
        {
            Games.Remove(gameToRemove);
        }
        DateUpdated = DateTime.UtcNow;
    }

    public void Inactive()
    {
        Active = false;
        DateUpdated = DateTime.UtcNow;
    }
}
