using MediatR;

namespace FiapCloudGamesCatalog.Domain.Events;

public class LibraryGameAddedDomainEvent : IStoredDomainEvent, INotification
{
    public DateTime OccurredOn { get; private set; }
    public Guid LibraryId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid GameId { get; private set; }

    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; }

    public LibraryGameAddedDomainEvent(Guid libraryId, Guid userId, Guid gameId)
    {
        LibraryId = libraryId;
        UserId = userId;
        GameId = gameId;
        OccurredOn = DateTime.UtcNow;

        AggregateId = gameId;
        AggregateType = "Library";
    }
}
