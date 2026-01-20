using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.Services;

public class AuditService : IAuditService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuditService> _logger;

    public AuditService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public AuditService(IUnitOfWork unitOfWork, ILogger<AuditService> logger)
    {
        this._unitOfWork = unitOfWork;
        this._logger = logger;
    }

    public async Task<IList<AuditGameUserCollectionResponseDto?>> GetByUserAsync(Guid userId, string? collection)
    {
        var listOfAudits = await _unitOfWork.AuditGameUsersRepo.GetByUserAsync(userId, Convert(collection));

        if (listOfAudits == null || !listOfAudits.Any()) return [];

        return listOfAudits.Select(x => (AuditGameUserCollectionResponseDto?)x).ToList();
    }

    public async Task<IList<AuditGameUserCollectionResponseDto?>> GetByGameAsync(Guid gameId, string? collection)
    {
        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId); 

        if (game == null)
        {
            _logger?.LogWarning("Game {GameId} not found when fetching audits by game", gameId);
            throw new ResourceNotFoundException(nameof(Game));
        }

        var listOfAudits = await _unitOfWork.AuditGameUsersRepo.GetByGameAsync(gameId, Convert(collection));

        if (listOfAudits == null || !listOfAudits.Any())
        {
            _logger?.LogInformation("No audits found for game {GameId}", gameId);
            return [];
        }

        _logger?.LogInformation("Retrieved {Count} audits for game {GameId}", listOfAudits.Count, gameId);
        return listOfAudits.Select(x => (AuditGameUserCollectionResponseDto?)x).ToList();
    }

    public async Task<IList<AuditGamePriceResponseDto?>> GetByGameAsync(Guid gameId)
    {
        var game = await _unitOfWork.GamesRepo.GetByIdAsync(gameId);

        if (game == null)
        {
            _logger?.LogWarning("Game {GameId} not found when fetching price audits", gameId);
            throw new ResourceNotFoundException(nameof(Game));
        }

        var listOfAudits = await _unitOfWork.AuditGamePriceRepo.GetByGameAsync(gameId);

        if (listOfAudits == null || !listOfAudits.Any())
        {
            _logger?.LogInformation("No price audits found for game {GameId}", gameId);
            return [];
        }

        _logger?.LogInformation("Retrieved {Count} price audits for game {GameId}", listOfAudits.Count, gameId);
        return listOfAudits.Select(x => (AuditGamePriceResponseDto?)x).ToList();
    }

    private AuditGameUserCollectionEnum? Convert(string? collection)
    {
        if (collection == null)
            return null;

        return (AuditGameUserCollectionEnum)Enum.Parse(typeof(AuditGameUserCollectionEnum), collection);
    }
}