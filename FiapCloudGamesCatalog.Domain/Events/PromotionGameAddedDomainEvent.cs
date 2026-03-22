using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class PromotionGameAddedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid PromotionId { get; private set; }
    public Guid GameId { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public PromotionGameAddedDomainEvent(Guid promotionId, Guid gameId)
    {
        PromotionId = promotionId;
        GameId = gameId;
        OccurredOn = DateTime.UtcNow;

        AggregateId = promotionId;
        AggregateType = "Promotion";
    }
}
