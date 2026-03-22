using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.Services;

public class PromotionService : IPromotionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PromotionService> _logger;

    public PromotionService(IUnitOfWork unitOfWork, ILogger<PromotionService> logger)
    {
        this._unitOfWork = unitOfWork;
        this._logger = logger;
    }

    public async Task<PromotionResponseDto?> AddAsync(PromotionRequestDto dto)
    {
        var games = new List<Game>();
        foreach (var gid in dto.GameIds)
        {
            var g = await _unitOfWork.GamesRepo.GetByIdAsync(gid);
            if (g != null) games.Add(g);
        }

        var promotion = new Promotion(games, dto.DiscountPercentage, dto.StartDate, dto.EndDate);
        await _unitOfWork.PromotionsRepo.AddAsync(promotion);
        await _unitOfWork.Commit();
        _logger.LogInformation("Created promotion {PromotionId} with {Count} games", promotion.Id, games.Count);
        return promotion;
    }

    public async Task<PromotionResponseDto?> GetAsync(Guid id)
    {
        var promotion = await _unitOfWork.PromotionsRepo.GetDetailedByIdAsync(id);

        if (promotion == null)
        {
            _logger.LogWarning("Promotion {PromotionId} not found", id);
            throw new ResourceNotFoundException(nameof(Promotion));
        }

        _logger.LogInformation("Retrieved promotion {PromotionId}", id);
        return promotion;
    }

    public async Task<IList<PromotionResponseDto?>> GetAllAsync()
    {
        var promotions = await _unitOfWork.PromotionsRepo.GetAllAsync();

        if (promotions == null || !promotions.Any())
        {
            _logger.LogInformation("No promotions found");
            return [];
        }

        _logger.LogInformation("Retrieved {Count} promotions", promotions.Count);
        return promotions.Select(x => (PromotionResponseDto?)x).ToList();
    }

    public async Task<IList<PromotionResponseDto?>> GetActiveAsync()
    {
        var promotions = await _unitOfWork.PromotionsRepo.GetActiveAsync();

        if (promotions == null || !promotions.Any())
        {
            _logger.LogInformation("No active promotions found");
            return [];
        }

        _logger.LogInformation("Retrieved {Count} active promotions", promotions.Count);
        return promotions.Select(x => (PromotionResponseDto?)x).ToList();
    }

    public async Task DeleteAsync(Guid id)
    {
        var promotion = await _unitOfWork.PromotionsRepo.GetByIdAsync(id);

        if (promotion == null)
        {
            _logger.LogWarning("Promotion {PromotionId} not found for delete", id);
            throw new ResourceNotFoundException(nameof(Promotion));
        }

        promotion.Delete();
        _unitOfWork.PromotionsRepo.Delete(promotion);
        await _unitOfWork.Commit();

        _logger.LogInformation("Deleted promotion {PromotionId}", id);
    }

    public async Task<PromotionResponseDto?> AddGameToPromotionAsync(Guid id, Guid gameId)
    {
        var promotion = await _unitOfWork.PromotionsRepo.GetDetailedByIdAsync(id);

        if (promotion == null)
        {
            _logger.LogWarning("Promotion {PromotionId} not found when adding game", id);
            throw new ResourceNotFoundException(nameof(Promotion));
        }

        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId);

        if (game == null)
        {
            _logger.LogWarning("Game {GameId} not found when adding to promotion {PromotionId}", gameId, id);
            throw new ResourceNotFoundException(nameof(Game));
        }

        promotion.AddGame(game);
        _unitOfWork.GamesRepo.Attach(game);
        _unitOfWork.PromotionsRepo.Update(promotion);
        await _unitOfWork.Commit();

        _logger.LogInformation("Added game {GameId} to promotion {PromotionId}", gameId, id);
        return promotion;
    }

    public async Task<PromotionResponseDto?> RemoveGameToPromotionAsync(Guid id, Guid gameId)
    {
        var promotion = await _unitOfWork.PromotionsRepo.GetDetailedByIdAsync(id);

        if (promotion == null)
        {
            _logger.LogWarning("Promotion {PromotionId} not found when removing game", id);
            throw new ResourceNotFoundException(nameof(Promotion));
        }

        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId);

        if (game == null)
        {
            _logger.LogWarning("Game {GameId} not found when removing from promotion {PromotionId}", gameId, id);
            throw new ResourceNotFoundException(nameof(Game));
        }

        promotion.RemoveGame(game);
        _unitOfWork.GamesRepo.Attach(game);
        _unitOfWork.PromotionsRepo.Update(promotion);
        await _unitOfWork.Commit();

        _logger.LogInformation("Removed game {GameId} from promotion {PromotionId}", gameId, id);
        return promotion;
    }
}