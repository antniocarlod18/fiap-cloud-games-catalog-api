using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class PromotionCreatedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid PromotionId { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public IReadOnlyList<Guid> GameIds { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public PromotionCreatedDomainEvent(Guid promotionId, decimal discountPercentage, DateTime startDate, DateTime endDate, IReadOnlyList<Guid> gameIds)
    {
        PromotionId = promotionId;
        DiscountPercentage = discountPercentage;
        StartDate = startDate;
        EndDate = endDate;
        GameIds = gameIds;
        OccurredOn = DateTime.UtcNow;

        AggregateId = promotionId;
        AggregateType = "Promotion";
    }
}
