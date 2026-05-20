using FiapCloudGamesCatalog.Domain.Events;

namespace FiapCloudGamesCatalog.Domain.Abstractions;

public interface IStoredEventPersistence
{
    Task AppendStoredDomainEventsAsync(
        IReadOnlyList<IStoredDomainEvent> events,
        CancellationToken cancellationToken = default);
}
