using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface IGameIndexer
{
    Task IndexAsync(Game game, CancellationToken cancellationToken = default);
}
