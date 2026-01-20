using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FiapCloudGamesCatalog.Application.Services;

public class LibraryService : ILibraryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LibraryService> _logger;

    public LibraryService(IUnitOfWork unitOfWork, ILogger<LibraryService> logger)
    {
        this._unitOfWork = unitOfWork;
        this._logger = logger;
    }   

    public async Task<LibraryResponseDto> GetLibraryByUserAsync(Guid userId)
    {
        var library = await _unitOfWork.LibrariesRepo.GetByUserAsync(userId);
        if (library == null)
        {
            _logger.LogWarning("Library not found for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Library));
        }

        _logger.LogInformation("Retrieved library for user {UserId}", userId);
        return library;
    }

    public async Task<LibraryResponseDto> CreateLibraryAsync(Guid userId)
    {
        var existingLibrary = await _unitOfWork.LibrariesRepo.GetByUserAsync(userId);
        if (existingLibrary != null)
        {
            _logger.LogWarning("Library already exists for user {UserId}", userId);
            throw new ResourceAlreadyExistsException(nameof(Library));
        }

        var library = new Library(userId, new List<Game>());

        await _unitOfWork.LibrariesRepo.AddAsync(library);
        await _unitOfWork.Commit();

        _logger.LogInformation("Created library for user {UserId}", userId);

        return library;
    }

    public async Task InactiveLibraryAsync(Guid userId)
    {
        var library = await _unitOfWork.LibrariesRepo.GetByUserAsync(userId);
        if (library == null)
        {
            _logger.LogWarning("Library not found when trying to inactive for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Library));
        }

        library.Inactive();

        _unitOfWork.LibrariesRepo.Update(library);
        await _unitOfWork.Commit();

        _logger.LogInformation("Inactivated library for user {UserId}", userId);
    }

    public async Task AddGamesToLibrary(Guid userId, IList<Guid> gameIds)
    {
        var library = await _unitOfWork.LibrariesRepo.GetByUserAsync(userId);
        if (library == null)
        {
            _logger.LogWarning("Library not found when adding games for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Library));
        }

        var games = await _unitOfWork.GamesRepo.GetByIds(gameIds);

        if (games == null || !games.Any())
        {
            _logger.LogWarning("Games not found when adding to library for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Game));
        }

        var gamesToAdd = games
            .ToList();

        foreach (var game in gamesToAdd)
        {
            library.Add(game);
            _unitOfWork.GamesRepo.Attach(game);
            await _unitOfWork.AuditGameUsersRepo.AddAsync(
                new AuditGameUserCollection(
                    userId,
                    game,
                    Domain.Enums.AuditGameUserActionEnum.Added,
                    Domain.Enums.AuditGameUserCollectionEnum.Library,
                    "Game added to library"));
        }

        _unitOfWork.LibrariesRepo.Update(library);

        await _unitOfWork.Commit();

        _logger.LogInformation("LibraryService: added {Count} games to library for user {UserId}", gamesToAdd.Count, userId);
    }

    public async Task RemoveGamesFromLibrary(Guid userId, IList<Guid> gameIds)
    {
        var library = await _unitOfWork.LibrariesRepo.GetByUserAsync(userId);
        if (library == null)
        {
            _logger.LogWarning("Library not found when removing games for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Library));
        }

        var games = await _unitOfWork.GamesRepo.GetByIds(gameIds);

        if (games == null || !games.Any())
        {
            _logger.LogWarning("Games not found when removing from library for user {UserId}", userId);
            throw new ResourceNotFoundException(nameof(Game));
        }

        var gamesToRemove = games
            .ToList();

        foreach (var game in gamesToRemove)
        {
            library.Remove(game);
            _unitOfWork.GamesRepo.Attach(game);
            await _unitOfWork.AuditGameUsersRepo.AddAsync(
                new AuditGameUserCollection(
                    userId,
                    game,
                    Domain.Enums.AuditGameUserActionEnum.Removed,
                    Domain.Enums.AuditGameUserCollectionEnum.Library,
                    "Game removed from library"));
        }

        _unitOfWork.LibrariesRepo.Update(library);

        await _unitOfWork.Commit();

        _logger.LogInformation("Removed {Count} games from library for user {UserId}", gamesToRemove.Count, userId);
    }
}