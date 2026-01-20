using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;

namespace FiapCloudGamesCatalog.Domain.Repositories;

public interface IAuditGamePriceRepository : IRepository<AuditGamePrice>
{
    Task<IList<AuditGamePrice>> GetByGameAsync(Guid gameId);
}