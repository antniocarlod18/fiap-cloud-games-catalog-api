namespace FiapCloudGamesCatalog.Domain.Events;

public interface IStoredDomainEvent : IDomainEvent
{
    Guid AggregateId { get; }
    string AggregateType { get; }
}
