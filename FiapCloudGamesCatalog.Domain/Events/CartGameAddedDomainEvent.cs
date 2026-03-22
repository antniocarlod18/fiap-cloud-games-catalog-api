using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class CartGameAddedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid CartId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid GameId { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public CartGameAddedDomainEvent(Guid cartId, Guid userId, Guid gameId)
    {
        CartId = cartId;
        UserId = userId;
        GameId = gameId;
        OccurredOn = DateTime.UtcNow;

        AggregateId = userId;
        AggregateType = "Cart";
    }
}
