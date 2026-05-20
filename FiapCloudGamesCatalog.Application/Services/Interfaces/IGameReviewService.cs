using FiapCloudGamesCatalog.Application.Dtos;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface IGameReviewService
{
    Task<GameReviewResponseDto> CreateAsync(Guid userId, Guid gameId, GameReviewRequestDto request);

    Task<GameReviewPageResponseDto> GetByGameAsync(Guid gameId, int skip, int take);
}
