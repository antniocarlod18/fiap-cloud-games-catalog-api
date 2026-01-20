using FiapCloudGamesCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesCatalog.Domain.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetByUserAsync(Guid userId);
    }
}
