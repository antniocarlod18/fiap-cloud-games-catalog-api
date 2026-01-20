using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;

namespace FiapCloudGamesCatalog.Domain.Repositories;

public interface IAuditGameUserCollectionRepository : IRepository<AuditGameUserCollection>
{
    Task<IList<AuditGameUserCollection>> GetByUserAsync(Guid userId, AuditGameUserCollectionEnum? collectionEnum);
    Task<IList<AuditGameUserCollection>> GetByGameAsync(Guid gameId, AuditGameUserCollectionEnum? collectionEnum);
}