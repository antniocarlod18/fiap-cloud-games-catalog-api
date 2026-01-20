using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface IGameService
{
    Task<GameResponseDto?> AddAsync(GameRequestDto gameRequestDto);
    Task<GameResponseDto?> GetAsync(Guid id);
    Task<IList<GameResponseDto?>> GetAllAvailableAsync();
    Task<GameResponseDto> GetByTitleAsync(string title);
    Task<IList<GameResponseDto?>> GetByGenreAsync(string genre);
    Task<GameResponseDto?> UpdateAsync(Guid id, GameRequestDto gameRequestDto);
    Task<IList<GameResponseDto?>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
