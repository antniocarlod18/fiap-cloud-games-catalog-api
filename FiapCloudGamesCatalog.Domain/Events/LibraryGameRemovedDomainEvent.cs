using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class LibraryGameRemovedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid LibraryId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid GameId { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public LibraryGameRemovedDomainEvent(Guid libraryId, Guid userId, Guid gameId)
    {
        LibraryId = libraryId;
        UserId = userId;
        GameId = gameId;
        OccurredOn = DateTime.UtcNow;

        AggregateId = userId;
        AggregateType = "Library";
    }
}
