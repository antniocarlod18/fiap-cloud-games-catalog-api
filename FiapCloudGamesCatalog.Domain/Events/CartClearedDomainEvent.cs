using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class CartClearedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid CartId { get; private set; }
    public Guid UserId { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public CartClearedDomainEvent(Guid cartId, Guid userId)
    {
        CartId = cartId;
        UserId = userId;
        OccurredOn = DateTime.UtcNow;

        AggregateId = userId;
        AggregateType = "Cart";
    }
}
