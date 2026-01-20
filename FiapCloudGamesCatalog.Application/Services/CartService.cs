using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FiapCloudGamesCatalog.Application.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CartService> _logger;

    public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
    {
        this._unitOfWork = unitOfWork;
        this._logger = logger;
    }

    public async Task<CartResponseDto> GetCartByUserAsync(Guid userId)
    {
        var cart = await _unitOfWork.CartsRepo.GetByUserAsync(userId);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Cart));
        }

        _logger.LogInformation("Retrieved cart for user {UserId}", userId);
        return cart;
    }

    public async Task<CartResponseDto> CreateCartAsync(Guid userId)
    {
        var existingCart = await _unitOfWork.CartsRepo.GetByUserAsync(userId);
        if (existingCart != null)
        {
            _logger.LogWarning("Cart already exists for user {UserId}", userId);
            throw new ResourceAlreadyExistsException(nameof(Cart));
        }

        var cart = new Cart(userId, new List<Game>());

        await _unitOfWork.CartsRepo.AddAsync(cart);
        await _unitOfWork.Commit();

        _logger.LogInformation("Created cart for user {UserId}", userId);

        return cart;
    }

    public async Task InactiveCartAsync(Guid userId)
    {
        var cart = await _unitOfWork.CartsRepo.GetByUserAsync(userId);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found when trying to inactive for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Library));
        }

        cart.Inactive();

        _unitOfWork.CartsRepo.Update(cart);
        await _unitOfWork.Commit();

        _logger.LogInformation("Inactivated cart for user {UserId}", userId);
    }

    public async Task AddGameToCart(Guid userId, Guid gameId)
    {
        var cart = await _unitOfWork.CartsRepo.GetByUserAsync(userId);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found when adding game for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Cart));
        }

        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId);

        if (game == null)
        {
            _logger.LogWarning("Game not found {GameId} when adding to cart for user {UserId}", gameId, userId);
            throw new ResourceNotFoundException(nameof(Game));
        }

        cart.AddGame(game);
        _unitOfWork.GamesRepo.Attach(game);
        _unitOfWork.CartsRepo.Update(cart);

        await _unitOfWork.AuditGameUsersRepo.AddAsync(
            new AuditGameUserCollection(
                userId,
                game,
                Domain.Enums.AuditGameUserActionEnum.Added,
                Domain.Enums.AuditGameUserCollectionEnum.Cart,
                "Game added to cart"));
        await _unitOfWork.Commit();

        _logger.LogInformation("Added game {GameId} to cart for user {UserId}", gameId, userId);
    }

    public async Task RemoveGameFromCart(Guid userId, Guid gameId)
    {
        var cart = await _unitOfWork.CartsRepo.GetByUserAsync(userId);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found when removing game for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Cart));
        }

        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId);

        if (game == null)
        {
            _logger.LogWarning("Game not found {GameId} when removing from cart for user {UserId}", gameId, userId);
            throw new ResourceNotFoundException(nameof(Game));
        }

        cart.RemoveGame(game);

        _unitOfWork.GamesRepo.Attach(game);
        _unitOfWork.CartsRepo.Update(cart);
        await _unitOfWork.AuditGameUsersRepo.AddAsync(
            new AuditGameUserCollection(
                userId, 
                game, 
                Domain.Enums.AuditGameUserActionEnum.Removed,
                Domain.Enums.AuditGameUserCollectionEnum.Cart,
                "Game removed from cart"));

        await _unitOfWork.Commit();

        _logger.LogInformation("Removed game {GameId} from cart for user {UserId}", gameId, userId);
    }

    public async Task AddGamesToCart(Guid userId, IList<Guid> gameIds)
    {
        var cart = await _unitOfWork.CartsRepo.GetByUserAsync(userId);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found when adding games for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Cart));
        }

        var games = await _unitOfWork.GamesRepo.GetByIds(gameIds);

        if (games == null || !games.Any())
        {
            _logger.LogWarning("Games not found when adding to cart for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Game));
        }

        var gamesToAdd = cart.Games
            .ToList();

        foreach (var game in gamesToAdd)
        {
            cart.AddGame(game);
            _unitOfWork.GamesRepo.Attach(game);
            await _unitOfWork.AuditGameUsersRepo.AddAsync(
                new AuditGameUserCollection(
                    userId,
                    game,
                    Domain.Enums.AuditGameUserActionEnum.Added,
                    Domain.Enums.AuditGameUserCollectionEnum.Cart,
                    "Game added to cart"));

        }

        _unitOfWork.CartsRepo.Update(cart);

        await _unitOfWork.Commit();

        _logger.LogInformation("Added {Count} games to cart for user {UserId}", gamesToAdd.Count, userId);
    }

    public async Task RemoveGamesFromCart(Guid userId, IList<Guid> gameIds)
    {
        var cart = await _unitOfWork.CartsRepo.GetByUserAsync(userId);
        if (cart == null)
        {
            _logger.LogWarning("Cart not found when removing games for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Cart));
        }

        if (!cart.Games.Any())
        {
            _logger.LogInformation("Cart is already empty for user {UserId}", userId);
            return;
        }

        var gamesToRemove = cart.Games
            .ToList();

        foreach (var game in gamesToRemove)
        {
            cart.RemoveGame(game);
            _unitOfWork.GamesRepo.Attach(game);
            await _unitOfWork.AuditGameUsersRepo.AddAsync(
                new AuditGameUserCollection(
                    userId,
                    game,
                    Domain.Enums.AuditGameUserActionEnum.Removed,
                    Domain.Enums.AuditGameUserCollectionEnum.Cart,
                    "Game removed from cart"));

        }

        _unitOfWork.CartsRepo.Update(cart);

        await _unitOfWork.Commit();

        _logger.LogInformation("Removed {Count} games from cart for user {UserId}", gamesToRemove.Count, userId);
    }
}