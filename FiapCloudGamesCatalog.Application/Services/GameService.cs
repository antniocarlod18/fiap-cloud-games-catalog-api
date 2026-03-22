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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GameService> _logger;

    public GameService(IUnitOfWork unitOfWork, ILogger<GameService> logger)
    {
        this._unitOfWork = unitOfWork;
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

        _logger.LogInformation("Created game {GameId} with title {Title}", game.Id, game.Title);

        return game;
    }

    public async Task<GameResponseDto?> GetAsync(Guid id)
    {
        var game = await _unitOfWork.GamesRepo.GetWithPromotionsByIdAsync(id);
        if(game == null)
        {
            _logger.LogWarning("Game {GameId} not found", id);
            throw new ResourceNotFoundException(nameof(Game));
        }

        _logger.LogInformation("Retrieved game {GameId}", id);
        return game;
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

        _logger.LogInformation("Updated game {GameId} with new title {Title}", id, game.Title);
        return game;
    }

    public async Task<IList<GameResponseDto?>> GetAllAvailableAsync()
    {
        var listOfAvailableGames = await _unitOfWork.GamesRepo.GetAvailableAsync();
        if (listOfAvailableGames == null || !listOfAvailableGames.Any())
        {
            _logger.LogInformation("No available games found");
            return [];
        }

        _logger.LogInformation("Retrieved {Count} available games", listOfAvailableGames.Count);
        return listOfAvailableGames.Select(x => (GameResponseDto?)x).ToList(); 
    }

    public async Task<GameResponseDto> GetByTitleAsync(string title)
    {
        var game = await _unitOfWork.GamesRepo.GetByTitleAsync(title);
        if (game == null)
        {
            _logger.LogWarning("Game with title {Title} not found", title);
            throw new ResourceNotFoundException(nameof(Game));
        }

        _logger.LogInformation("Retrieved game by title {Title}", title);
        return game;
    }

    public async Task<IList<GameResponseDto?>> GetByGenreAsync(string genre)
    {
        var listOfGames = await _unitOfWork.GamesRepo.GetByGenreAsync(genre);
        if (listOfGames == null || !listOfGames.Any())
        {
            _logger.LogInformation("No games found for genre {Genre}", genre);
            return [];
        }

        _logger.LogInformation("Retrieved {Count} games for genre {Genre}", listOfGames.Count, genre);
        return listOfGames.Select(x => (GameResponseDto?)x).ToList();
    }

    public async Task<IList<GameResponseDto?>> GetAllAsync()
    {
        var listOfGames = await _unitOfWork.GamesRepo.GetAllAsync();
        if (listOfGames == null || !listOfGames.Any())
        {
            _logger.LogInformation("No games found");
            return [];
        }

        _logger.LogInformation("Retrieved {Count} games", listOfGames.Count);
        return listOfGames.Select(x => (GameResponseDto?)x).ToList();
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

        _logger.LogInformation("Marked game {GameId} as unavailable", id);
    }
}
