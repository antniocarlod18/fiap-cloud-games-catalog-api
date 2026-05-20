namespace FiapCloudGamesCatalog.Domain.Documents;

public class MongoStoredEventDocument
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; } = string.Empty;
    public int Version { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public string Data { get; set; } = string.Empty;
    public string? Metadata { get; set; }
    public DateTime DateCreated { get; set; }
}
