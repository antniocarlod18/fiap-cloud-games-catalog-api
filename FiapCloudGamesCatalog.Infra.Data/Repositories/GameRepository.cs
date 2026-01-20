using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesCatalog.Infra.Data.Repositories;

public class GameRepository : Repository<Game>, IGameRepository
{
    private readonly ContextDb _context;

    public GameRepository(ContextDb context) : base(context)
    {
        _context = context;
    }

    public async Task<Game?> GetByTitleAsync(string title)
    {
        return await _context.Games
            .AsNoTracking()
            .Include(g => g.Promotions)
            .FirstOrDefaultAsync(g => g.Title == title);
    }

    public async Task<IList<Game>> GetAvailableAsync()
    {
        return await _context.Games
            .AsNoTracking()
            .Include(g => g.Promotions)
            .Where(g => g.Available == true)
            .ToListAsync();
    }

    public new async Task<IList<Game>> GetAllAsync()
    {
        return await _context.Games
            .AsNoTracking()
            .Include(g => g.Promotions)
            .ToListAsync();
    }

    public async Task<IList<Game>> GetByGenreAsync(string genre)
    {
        return await _context.Games
            .AsNoTracking()
            .Include(g => g.Promotions)
            .Where(g => g.Genre == genre)
            .ToListAsync();
    }

    public async Task<Game?> GetWithPromotionsByIdAsync(Guid idGame)
    {
        return await _context.Games
            .Include(g => g.Promotions)
            .FirstOrDefaultAsync(g => g.Id == idGame);
    }

    public async Task<IList<Game>> GetByIds(IList<Guid> idsGame)
    {
        return await _context.Games
            .Include(g => g.Promotions)
            .Where(g => idsGame.Contains(g.Id))
            .ToListAsync();
    }
}
