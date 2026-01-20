using FiapCloudGamesCatalog.Domain.Entities;
using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events
{
    public class OrderCompleteDomainEvent : IDomainEvent, INotification
    {
        public DateTime OccurredOn { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid UserId { get; private set; }
        public IList<OrderGameItem> Games { get; set; }
        public OrderCompleteDomainEvent(Guid orderId, Guid userId, IList<OrderGameItem> games)
        {
            OrderId = orderId;
            UserId = userId;
            Games = games;
            OccurredOn = DateTime.UtcNow;
        }    
    }
}
