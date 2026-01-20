using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Dtos;

public class PromotionResponseDto
{
    public Guid Id { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Active { get; set; }
    public IList<GameResponseDto> Games { get; set; } = [];

    public static implicit operator PromotionResponseDto?(Promotion? p)
    {
        if (p == null) return null;
        return new PromotionResponseDto
        {
            Id = p.Id,
            DiscountPercentage = p.DiscountPercentage,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Active = p.Active,
            Games = p.Games.Select(o => (GameResponseDto)o!).ToList()
        };
    }
}