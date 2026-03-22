using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class LibraryCreatedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid LibraryId { get; private set; }
    public Guid UserId { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public LibraryCreatedDomainEvent(Guid libraryId, Guid userId)
    {
        LibraryId = libraryId;
        UserId = userId;
        OccurredOn = DateTime.UtcNow;

        AggregateId = userId;
        AggregateType = "Library";
    }
}
