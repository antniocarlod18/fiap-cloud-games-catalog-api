using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events
{
    public class OrderRefundedDomainEvent : IStoredDomainEvent, INotification
    {
        public DateTime OccurredOn { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid UserId { get; private set; }
        public IReadOnlyList<Guid> GameIds { get; private set; }
        public Guid AggregateId { get; private set; }
        public string AggregateType { get; private set; }

        public OrderRefundedDomainEvent(Guid orderId, Guid userId, IReadOnlyList<Guid> gameIds)
        {
            OrderId = orderId;
            UserId = userId;
            GameIds = gameIds;
            OccurredOn = DateTime.UtcNow;
            AggregateId = userId;
            AggregateType = "Order";
        }    
    }
}
