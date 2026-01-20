using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesCatalog.Infra.Data.Repositories;

public class LibraryRepository : Repository<Library>, ILibraryRepository
{
    private readonly ContextDb _context;

    public LibraryRepository(ContextDb context) : base(context)
    {
        _context = context;
    }

    public async Task<Library?> GetByUserAsync(Guid userId)
    {
        return await _context.Libraries
            .Include(o => o.Games)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Active);
    }
}
