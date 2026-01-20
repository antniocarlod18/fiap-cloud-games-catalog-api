using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface IPromotionService
{
    Task<PromotionResponseDto?> AddAsync(PromotionRequestDto dto);
    Task<PromotionResponseDto?> GetAsync(Guid id);
    Task<IList<PromotionResponseDto?>> GetActiveAsync();
    Task DeleteAsync(Guid id);
    Task<PromotionResponseDto?> AddGameToPromotionAsync(Guid id, Guid gameId);
    Task<PromotionResponseDto?> RemoveGameToPromotionAsync(Guid id, Guid gameId);
    Task<IList<PromotionResponseDto?>> GetAllAsync();
}