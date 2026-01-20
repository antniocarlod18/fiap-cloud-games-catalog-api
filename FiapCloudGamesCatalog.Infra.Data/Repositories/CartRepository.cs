using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesCatalog.Infra.Data.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    private readonly ContextDb _context;

    public CartRepository(ContextDb context) : base(context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserAsync(Guid userId)
    {
        return await _context.Carts
            .Include(o => o.Games)
                .ThenInclude(g => g.Promotions)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Active);
    }
}
