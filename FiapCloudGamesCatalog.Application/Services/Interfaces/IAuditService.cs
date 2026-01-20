using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface IAuditService
{
    Task<IList<AuditGameUserCollectionResponseDto?>> GetByUserAsync(Guid userId, string? collection);
    Task<IList<AuditGameUserCollectionResponseDto?>> GetByGameAsync(Guid gameId, string? collection);
    Task<IList<AuditGamePriceResponseDto?>> GetByGameAsync(Guid gameId);
}