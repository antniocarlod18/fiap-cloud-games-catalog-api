using System.Linq;
using System.Text.Json;
using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Documents;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FiapCloudGamesCatalog.Infra.Data.Mongo;

public class StoredEventPersistence : IStoredEventPersistence
{
    private readonly IMongoCollection<MongoStoredEventDocument> _collection;
    private readonly ILogger<StoredEventPersistence> _logger;

    public StoredEventPersistence(IMongoDatabase database, ILogger<StoredEventPersistence> logger)
    {
        _collection = database.GetCollection<MongoStoredEventDocument>("stored_events");
        _logger = logger;
    }

    public async Task AppendStoredDomainEventsAsync(
        IReadOnlyList<IStoredDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        if (events.Count == 0)
            return;

        var eventsByAggregate = events
            .GroupBy(e => new { e.AggregateId, e.AggregateType })
            .ToList();

        foreach (var group in eventsByAggregate)
        {
            var filter = Builders<MongoStoredEventDocument>.Filter.And(
                Builders<MongoStoredEventDocument>.Filter.Eq(x => x.AggregateId, group.Key.AggregateId),
                Builders<MongoStoredEventDocument>.Filter.Eq(x => x.AggregateType, group.Key.AggregateType));

            var sort = Builders<MongoStoredEventDocument>.Sort.Descending(x => x.Version);
            var last = await _collection
                .Find(filter)
                .Sort(sort)
                .Limit(1)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            var nextVersion = (last?.Version ?? 0) + 1;
            var documents = new List<MongoStoredEventDocument>();

            foreach (var domainEvent in group)
            {
                var eventType = domainEvent.GetType();
                var eventTypeName = eventType.AssemblyQualifiedName ?? eventType.FullName ?? eventType.Name;
                try
                {
                    var data = JsonSerializer.Serialize(domainEvent, eventType);
                    documents.Add(new MongoStoredEventDocument
                    {
                        Id = Guid.NewGuid(),
                        AggregateId = group.Key.AggregateId,
                        AggregateType = group.Key.AggregateType,
                        Version = nextVersion++,
                        EventType = eventTypeName,
                        OccurredOn = domainEvent.OccurredOn,
                        Data = data,
                        Metadata = null,
                        DateCreated = DateTime.UtcNow
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to serialize domain event {EventType} for aggregate {AggregateId}",
                        eventType.Name, group.Key.AggregateId);
                    throw;
                }
            }

            if (documents.Count > 0)
                await _collection.InsertManyAsync(documents, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
