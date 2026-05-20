using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Repositories;
using MongoDB.Driver;

namespace FiapCloudGamesCatalog.Infra.Data.Mongo;

public class GameReviewRepository : IGameReviewRepository
{
    private readonly IMongoCollection<GameReview> _collection;

    public GameReviewRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<GameReview>("game_reviews");
    }

    public async Task AddAsync(GameReview review, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(review, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<GameReview>> ListByGameIdAsync(
        Guid gameId,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        var takeClamped = Math.Clamp(take, 1, 100);
        var skipSafe = Math.Max(0, skip);

        return await _collection
            .Find(x => x.GameId == gameId)
            .SortByDescending(x => x.CreatedAtUtc)
            .Skip(skipSafe)
            .Limit(takeClamped)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<long> CountByGameIdAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(x => x.GameId == gameId, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
