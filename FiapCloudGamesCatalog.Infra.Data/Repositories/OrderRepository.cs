using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesCatalog.Infra.Data.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly ContextDb _context;

    public OrderRepository(ContextDb context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<Order>> GetByUserAsync(Guid userId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.Games)
                .ThenInclude(og => og.Game)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<Order?> GetDetailedByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.Games)
                .ThenInclude(og => og.Game)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
