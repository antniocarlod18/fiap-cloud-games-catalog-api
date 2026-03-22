using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class OrderCreatedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public IReadOnlyList<Guid> GameIds { get; private set; }
    public decimal Price { get; private set; }
    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public OrderCreatedDomainEvent(Guid orderId, Guid userId, IReadOnlyList<Guid> gameIds, decimal price)
    {
        OrderId = orderId;
        UserId = userId;
        GameIds = gameIds;
        Price = price;
        OccurredOn = DateTime.UtcNow;
        AggregateId = userId;
        AggregateType = "Order";
    }    
}
