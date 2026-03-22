using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class GamePriceUpdatedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid GameId { get; private set; }
    public decimal OldPrice { get; private set; }
    public decimal NewPrice { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public GamePriceUpdatedDomainEvent(Guid gameId, decimal oldPrice, decimal newPrice)
    {
        GameId = gameId;
        OldPrice = oldPrice;
        NewPrice = newPrice;
        OccurredOn = DateTime.UtcNow;

        AggregateId = gameId;
        AggregateType = "Game";
    }
}
