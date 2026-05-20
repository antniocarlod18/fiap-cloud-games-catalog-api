using FiapCloudGamesCatalog.Application.Caching;
using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.Services;

public class GameService : IGameService
{
    private static readonly TimeSpan GameByIdCacheTtl = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan GameByTitleCacheTtl = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan GameListCacheTtl = TimeSpan.FromMinutes(10);

    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameIndexer _gameIndexer;
    private readonly ICacheService _cache;
    private readonly ILogger<GameService> _logger;

    public GameService(IUnitOfWork unitOfWork, IGameIndexer gameIndexer, ICacheService cache, ILogger<GameService> logger)
    {
        this._unitOfWork = unitOfWork;
        this._gameIndexer = gameIndexer;
        this._cache = cache;
        this._logger = logger;
    }

    public async Task<GameResponseDto?> AddAsync(GameRequestDto gameRequestDto)
    {
        var platforms = gameRequestDto.GamePlatforms
            .Select(s => (GamePlatformEnum)Enum.Parse(typeof(GamePlatformEnum), s))
            .ToList();

        var game = new Game(
            gameRequestDto.Title,
            gameRequestDto.Genre,
            platforms,
            gameRequestDto.Description,
            gameRequestDto.Price,
            gameRequestDto.Developer,
            gameRequestDto.Distributor,
            gameRequestDto.GameVersion,
            gameRequestDto.Available
        );

        await _unitOfWork.GamesRepo.AddAsync(game);
        await _unitOfWork.Commit();

        await TryIndexGameAsync(game);
        await InvalidateGamesAndSearchCacheAsync();

        _logger.LogInformation("Created game {GameId} with title {Title}", game.Id, game.Title);

        return game;
    }

    public async Task<GameResponseDto?> GetAsync(Guid id)
    {
        var cacheKey = DistributedCacheKeys.GameById(id);
        var cached = await _cache.GetAsync<GameResponseDto>(cacheKey);
        if (cached != null)
            return cached;

        var game = await _unitOfWork.GamesRepo.GetWithPromotionsByIdAsync(id);
        if(game == null)
        {
            _logger.LogWarning("Game {GameId} not found", id);
            throw new ResourceNotFoundException(nameof(Game));
        }

        var dto = (GameResponseDto?)game;
        if (dto != null)
            await _cache.SetAsync(cacheKey, dto, GameByIdCacheTtl);

        _logger.LogInformation("Retrieved game {GameId}", id);
        return dto;
    }

    public async Task<GameResponseDto?> UpdateAsync(Guid id, GameRequestDto gameRequestDto)
    {
        var game = await _unitOfWork.GamesRepo.GetWithPromotionsByIdAsync(id);

        if (game == null)
        {
            _logger.LogWarning("Game {GameId} not found for update", id);
            throw new ResourceNotFoundException(nameof(Game));
        }

        var platforms = gameRequestDto.GamePlatforms
            .Select(s => (GamePlatformEnum)Enum.Parse(typeof(GamePlatformEnum), s))
            .ToList();

        game.UpdateDetails(
            gameRequestDto.Title,
            gameRequestDto.Genre,
            platforms,
            gameRequestDto.Description,
            gameRequestDto.Developer,
            gameRequestDto.Distributor,
            gameRequestDto.GameVersion,
            gameRequestDto.Available);

        if(game.Price != gameRequestDto.Price)
        {
            var oldPrice = game.Price;
            game.UpdatePrice(gameRequestDto.Price);

            var audit = new AuditGamePrice(game, oldPrice, gameRequestDto.Price);
            await _unitOfWork.AuditGamePriceRepo.AddAsync(audit);
        }

        _unitOfWork.GamesRepo.Update(game);
        await _unitOfWork.Commit();

        await TryIndexGameAsync(game);
        await InvalidateGamesAndSearchCacheAsync();

        _logger.LogInformation("Updated game {GameId} with new title {Title}", id, game.Title);
        return game;
    }

    public async Task<IList<GameResponseDto?>> GetAllAvailableAsync()
    {
        var cacheKey = DistributedCacheKeys.GamesAvailable();
        var cached = await _cache.GetAsync<List<GameResponseDto?>>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Retrieved {Count} available games from Redis cache", cached.Count);
            return cached;
        }

        var listOfAvailableGames = await _unitOfWork.GamesRepo.GetAvailableAsync();
        if (listOfAvailableGames == null || !listOfAvailableGames.Any())
        {
            _logger.LogInformation("No available games found");
            var empty = new List<GameResponseDto?>();
            await _cache.SetAsync(cacheKey, empty, GameListCacheTtl);
            return empty;
        }

        var result = listOfAvailableGames.Select(x => (GameResponseDto?)x).ToList();
        await _cache.SetAsync(cacheKey, result, GameListCacheTtl);

        _logger.LogInformation("Retrieved {Count} available games", listOfAvailableGames.Count);
        return result;
    }

    public async Task<GameResponseDto> GetByTitleAsync(string title)
    {
        var cacheKey = DistributedCacheKeys.GameByTitle(title);
        var cached = await _cache.GetAsync<GameResponseDto>(cacheKey);
        if (cached != null)
            return cached;

        var game = await _unitOfWork.GamesRepo.GetByTitleAsync(title);
        if (game == null)
        {
            _logger.LogWarning("Game with title {Title} not found", title);
            throw new ResourceNotFoundException(nameof(Game));
        }

        GameResponseDto? dtoNullable = game;
        var dto = dtoNullable!;
        await _cache.SetAsync(cacheKey, dto, GameByTitleCacheTtl);

        _logger.LogInformation("Retrieved game by title {Title}", title);
        return dto;
    }

    public async Task<IList<GameResponseDto?>> GetByGenreAsync(string genre)
    {
        var cacheKey = DistributedCacheKeys.GamesByGenre(genre);
        var cached = await _cache.GetAsync<List<GameResponseDto?>>(cacheKey);
        if (cached != null)
            return cached;

        var listOfGames = await _unitOfWork.GamesRepo.GetByGenreAsync(genre);
        if (listOfGames == null || !listOfGames.Any())
        {
            _logger.LogInformation("No games found for genre {Genre}", genre);
            var empty = new List<GameResponseDto?>();
            await _cache.SetAsync(cacheKey, empty, GameListCacheTtl);
            return empty;
        }

        var result = listOfGames.Select(x => (GameResponseDto?)x).ToList();
        await _cache.SetAsync(cacheKey, result, GameListCacheTtl);

        _logger.LogInformation("Retrieved {Count} games for genre {Genre}", listOfGames.Count, genre);
        return result;
    }

    public async Task<IList<GameResponseDto?>> GetAllAsync()
    {
        var cacheKey = DistributedCacheKeys.GamesAll();
        var cached = await _cache.GetAsync<List<GameResponseDto?>>(cacheKey);
        if (cached != null)
            return cached;

        var listOfGames = await _unitOfWork.GamesRepo.GetAllAsync();
        if (listOfGames == null || !listOfGames.Any())
        {
            _logger.LogInformation("No games found");
            var empty = new List<GameResponseDto?>();
            await _cache.SetAsync(cacheKey, empty, GameListCacheTtl);
            return empty;
        }

        var result = listOfGames.Select(x => (GameResponseDto?)x).ToList();
        await _cache.SetAsync(cacheKey, result, GameListCacheTtl);

        _logger.LogInformation("Retrieved {Count} games", listOfGames.Count);
        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        var game = await _unitOfWork.GamesRepo.GetByIdAsync(id);
        if (game == null)
        {
            _logger.LogWarning("Game {GameId} not found for delete", id);
            throw new ResourceNotFoundException(nameof(Game));
        }

        game.Available = false;
        _unitOfWork.GamesRepo.Update(game);
        await _unitOfWork.Commit();

        await TryIndexGameAsync(game);
        await InvalidateGamesAndSearchCacheAsync();

        _logger.LogInformation("Marked game {GameId} as unavailable", id);
    }

    private async Task InvalidateGamesAndSearchCacheAsync()
    {
        await _cache.RemoveByPrefixAsync(DistributedCacheKeys.GamesPrefix);
        await _cache.RemoveByPrefixAsync(DistributedCacheKeys.SearchPrefix);
    }

    private async Task TryIndexGameAsync(Game game)
    {
        try
        {
            await _gameIndexer.IndexAsync(game);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Elasticsearch indexing failed for game {GameId}", game.Id);
        }
    }
}
