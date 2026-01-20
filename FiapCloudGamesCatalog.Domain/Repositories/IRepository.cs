using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Domain.Repositories;

public interface IRepository<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Attach(T entity);
}
