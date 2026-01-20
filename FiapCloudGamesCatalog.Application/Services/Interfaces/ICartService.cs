using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface ICartService
{
    Task<CartResponseDto> GetCartByUserAsync(Guid userId);
    Task<CartResponseDto> CreateCartAsync(Guid userId);
    Task AddGameToCart(Guid id, Guid gameId);
    Task RemoveGameFromCart(Guid id, Guid gameId);
    Task AddGamesToCart(Guid userId, IList<Guid> gameIds);
    Task RemoveGamesFromCart(Guid userId, IList<Guid> gameIds);
    Task InactiveCartAsync(Guid userId);
}