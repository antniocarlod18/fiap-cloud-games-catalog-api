using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesCatalog.Infra.Data.Repositories;

public class AuditGameUserCollectionRepository : Repository<AuditGameUserCollection>, IAuditGameUserCollectionRepository
{
    private readonly ContextDb _context;

    public AuditGameUserCollectionRepository(ContextDb context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<AuditGameUserCollection>> GetByUserAsync(Guid userId, AuditGameUserCollectionEnum? collectionEnum)
    {
        var query = _context.Set<AuditGameUserCollection>()
            .AsNoTracking()
            .Include(a => a.Game)
            .Where(a => a.UserId == userId);

        if (collectionEnum.HasValue)
            query = query.Where(a => a.Collection == collectionEnum.Value);

        return await query.ToListAsync();
    }

    public async Task<IList<AuditGameUserCollection>> GetByGameAsync(Guid gameId, AuditGameUserCollectionEnum? collectionEnum)
    {
        var query = _context.Set<AuditGameUserCollection>()
            .AsNoTracking()
            .Include(a => a.Game)
            .Where(a => a.Game.Id == gameId);

        if (collectionEnum.HasValue)
            query = query.Where(a => a.Collection == collectionEnum.Value);

        return await query.ToListAsync();
    }
}
