using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Domain.Repositories;

public interface IGameReviewRepository
{
    Task AddAsync(GameReview review, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GameReview>> ListByGameIdAsync(
        Guid gameId,
        int skip,
        int take,
        CancellationToken cancellationToken = default);

    Task<long> CountByGameIdAsync(Guid gameId, CancellationToken cancellationToken = default);
}
