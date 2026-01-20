using FiapCloudGamesCatalog.Application.Dtos;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface ILibraryService
{
    Task<LibraryResponseDto> GetLibraryByUserAsync(Guid userId);
    Task<LibraryResponseDto> CreateLibraryAsync(Guid userId);
    Task AddGamesToLibrary(Guid userId, IList<Guid> gameIds);
    Task RemoveGamesFromLibrary(Guid userId, IList<Guid> gameIds);
    Task InactiveLibraryAsync(Guid userId);
}
