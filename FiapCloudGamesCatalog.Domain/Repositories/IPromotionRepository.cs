using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Domain.Repositories;

public interface IPromotionRepository : IRepository<Promotion>
{
    Task<IList<Promotion>> GetActiveAsync();
    Task<Promotion?> GetDetailedByIdAsync(Guid id);
    new Task<IList<Promotion>> GetAllAsync();
}   