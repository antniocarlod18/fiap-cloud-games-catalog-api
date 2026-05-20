using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.Services;

public class GameReviewService : IGameReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameReviewRepository _gameReviewRepository;
    private readonly ILogger<GameReviewService> _logger;

    public GameReviewService(
        IUnitOfWork unitOfWork,
        IGameReviewRepository gameReviewRepository,
        ILogger<GameReviewService> logger)
    {
        _unitOfWork = unitOfWork;
        _gameReviewRepository = gameReviewRepository;
        _logger = logger;
    }

    public async Task<GameReviewResponseDto> CreateAsync(Guid userId, Guid gameId, GameReviewRequestDto request)
    {
        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId);
        if (game == null)
            throw new ResourceNotFoundException(nameof(Game));

        var review = new GameReview
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            UserId = userId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _gameReviewRepository.AddAsync(review);

        _logger.LogInformation("User {UserId} created review {ReviewId} for game {GameId}", userId, review.Id, gameId);

        return ToDto(review);
    }

    public async Task<GameReviewPageResponseDto> GetByGameAsync(Guid gameId, int skip, int take)
    {
        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId);
        if (game == null)
            throw new ResourceNotFoundException(nameof(Game));

        var pageSize = take <= 0 ? 20 : Math.Min(take, 100);
        var offset = Math.Max(0, skip);

        var total = await _gameReviewRepository.CountByGameIdAsync(gameId);
        var items = await _gameReviewRepository.ListByGameIdAsync(gameId, offset, pageSize);

        return new GameReviewPageResponseDto
        {
            Items = items.Select(ToDto).ToList(),
            TotalCount = total
        };
    }

    private static GameReviewResponseDto ToDto(GameReview r) => new()
    {
        Id = r.Id,
        GameId = r.GameId,
        UserId = r.UserId,
        Rating = r.Rating,
        Comment = r.Comment,
        CreatedAtUtc = r.CreatedAtUtc
    };
}
