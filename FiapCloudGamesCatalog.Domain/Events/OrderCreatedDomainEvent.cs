using FiapCloudGamesCatalog.Domain.Entities;
using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class OrderCreatedDomainEvent : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public IList<OrderGameItem> Games { get; set; }
    public decimal Price { get; set; }
    public OrderCreatedDomainEvent(Guid orderId, Guid userId, IList<OrderGameItem> games, decimal price)
    {
        OrderId = orderId;
        UserId = userId;
        Games = games;
        Price = price;
        OccurredOn = DateTime.UtcNow;
    }    
}
