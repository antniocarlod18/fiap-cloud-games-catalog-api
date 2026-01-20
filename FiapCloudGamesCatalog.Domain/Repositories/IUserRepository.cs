using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Domain.Repositories;

public interface IUserRepository : IRepository<Cart>
{
    Task<Cart?> GetByEmailAsync(string email);
    Task<IList<Cart>> GetActiveAsync();
    Task<Cart?> GetDetailedByIdAsync(Guid userId);
}   