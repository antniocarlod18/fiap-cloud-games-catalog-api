using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class PromotionDeletedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid PromotionId { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public PromotionDeletedDomainEvent(Guid promotionId)
    {
        PromotionId = promotionId;
        OccurredOn = DateTime.UtcNow;

        AggregateId = promotionId;
        AggregateType = "Promotion";
    }
}
