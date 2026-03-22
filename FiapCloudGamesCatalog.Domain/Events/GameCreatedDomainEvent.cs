using FiapCloudGamesCatalog.Domain.Enums;
using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class GameCreatedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid GameId { get; private set; }
    public string Title { get; private set; }
    public string Genre { get; private set; }
    public decimal Price { get; private set; }
    public bool Available { get; private set; }
    public IReadOnlyList<GamePlatformEnum> GamePlatforms { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public GameCreatedDomainEvent(Guid gameId, string title, string genre, decimal price, bool available, IReadOnlyList<GamePlatformEnum> gamePlatforms)
    {
        GameId = gameId;
        Title = title;
        Genre = genre;
        Price = price;
        Available = available;
        GamePlatforms = gamePlatforms;
        OccurredOn = DateTime.UtcNow;

        AggregateId = gameId;
        AggregateType = "Game";
    }
}
