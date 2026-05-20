using FiapCloudGamesCatalog.Application.Dtos;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface IGameSearchService
{
    Task<IReadOnlyList<GameSearchHitDto>> SearchAsync(string query, int size = 20, bool onlyAvailable = true, CancellationToken cancellationToken = default);
}
