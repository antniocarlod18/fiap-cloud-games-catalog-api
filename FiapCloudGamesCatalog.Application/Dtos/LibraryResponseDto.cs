using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;

namespace FiapCloudGamesCatalog.Application.Dtos;

public class LibraryResponseDto
{
    public Guid UserId { get; set; }
    public IList<GameResponseDto?> Games { get; set; } = [];

    public static implicit operator LibraryResponseDto?(Library? library)
    {
        if (library == null) return null;

        return new LibraryResponseDto
        {
            UserId = library.UserId,
            Games = library.Games.Select(x => (GameResponseDto?)x).ToList()
        };
    }
}