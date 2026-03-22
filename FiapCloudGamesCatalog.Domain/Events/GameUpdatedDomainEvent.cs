using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class GameUpdatedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid GameId { get; private set; }
    public string Title { get; private set; }
    public string Genre { get; private set; }
    public bool Available { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public GameUpdatedDomainEvent(Guid gameId, string title, string genre, bool available)
    {
        GameId = gameId;
        Title = title;
        Genre = genre;
        Available = available;
        OccurredOn = DateTime.UtcNow;

        AggregateId = gameId;
        AggregateType = "Game";
    }
}
