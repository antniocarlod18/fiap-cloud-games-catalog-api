using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGamesCatalog.Application.Dtos;

public class AuditGamePriceResponseDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public string? Justification { get; set; }
    public DateTime DateCreated { get; set; }

    public static implicit operator AuditGamePriceResponseDto?(AuditGamePrice? audit)
    {
        if (audit == null) return null;

        return new AuditGamePriceResponseDto
        {
            Id = audit.Id,
            GameId = audit.Game.Id,
            OldPrice = audit.OldPrice,
            NewPrice = audit.NewPrice,
            Justification = audit.Justification,
            DateCreated = audit.DateCreated
        };
    }
}