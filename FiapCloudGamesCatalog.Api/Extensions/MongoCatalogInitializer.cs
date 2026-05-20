using FiapCloudGamesCatalog.Domain.Documents;
using FiapCloudGamesCatalog.Domain.Entities;
using MongoDB.Driver;

namespace FiapCloudGamesCatalog.Api.Extensions;

public class MongoCatalogInitializer : IHostedService
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoCatalogInitializer> _logger;

    public MongoCatalogInitializer(IMongoDatabase database, ILogger<MongoCatalogInitializer> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var storedEvents = _database.GetCollection<MongoStoredEventDocument>(MongoCollectionNames.StoredEvents);
        var storedIndexKeys = Builders<MongoStoredEventDocument>.IndexKeys
            .Ascending(x => x.AggregateId)
            .Ascending(x => x.AggregateType)
            .Ascending(x => x.Version);
        var storedIndexModel = new CreateIndexModel<MongoStoredEventDocument>(
            storedIndexKeys,
            new CreateIndexOptions { Unique = true, Name = "ix_aggregate_version" });
        await TryCreateIndexAsync(
            () => storedEvents.Indexes.CreateOneAsync(storedIndexModel, cancellationToken: cancellationToken),
            "stored_events.ix_aggregate_version",
            cancellationToken).ConfigureAwait(false);

        var reviews = _database.GetCollection<GameReview>(MongoCollectionNames.GameReviews);
        var reviewKeys = Builders<GameReview>.IndexKeys.Ascending(x => x.GameId).Descending(x => x.CreatedAtUtc);
        var reviewModel = new CreateIndexModel<GameReview>(reviewKeys, new CreateIndexOptions { Name = "ix_game_created" });
        await TryCreateIndexAsync(
            () => reviews.Indexes.CreateOneAsync(reviewModel, cancellationToken: cancellationToken),
            "game_reviews.ix_game_created",
            cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("MongoDB catalog indexes ensured (stored_events, game_reviews).");
    }

    private async Task TryCreateIndexAsync(Func<Task> createIndex, string indexLabel, CancellationToken cancellationToken)
    {
        try
        {
            await createIndex().ConfigureAwait(false);
        }
        catch (MongoException ex)
        {
            _logger.LogDebug(ex, "Mongo index {IndexLabel} may already exist; skipping. {Message}", indexLabel, ex.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
