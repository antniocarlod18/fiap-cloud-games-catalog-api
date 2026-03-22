using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class StoredEvent : EntityBase
{
    public required Guid AggregateId { get; set; }
    public required string AggregateType { get; set; } = string.Empty;
    public required int Version { get; set; }
    public required string EventType { get; set; } = string.Empty;
    public required DateTime OccurredOn { get; set; }
    public required string Data { get; set; } = string.Empty;
    public string? Metadata { get; set; }

    [SetsRequiredMembers]
    public StoredEvent() : base()
    {
    }
}
