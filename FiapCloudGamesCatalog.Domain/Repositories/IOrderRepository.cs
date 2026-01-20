using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IList<Order>> GetByUserAsync(Guid userId);
    Task<Order?> GetDetailedByIdAsync(Guid id);
}