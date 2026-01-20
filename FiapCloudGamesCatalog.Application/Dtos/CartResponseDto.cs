using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;

namespace FiapCloudGamesCatalog.Application.Dtos;

public class CartResponseDto
{
    public Guid UserId { get; set; }
    public IList<GameResponseDto?> Games { get; set; } = [];

    public static implicit operator CartResponseDto?(Cart? cart)
    {
        if (cart == null) return null;

        return new CartResponseDto
        {
            UserId = cart.UserId,
            Games = cart.Games.Select(x => (GameResponseDto?)x).ToList()
        };
    }
}