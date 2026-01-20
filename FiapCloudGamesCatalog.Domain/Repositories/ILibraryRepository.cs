using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Domain.Repositories
{
    public interface ILibraryRepository : IRepository<Library>
    {
        Task<Library?> GetByUserAsync(Guid userId);
    }
}
