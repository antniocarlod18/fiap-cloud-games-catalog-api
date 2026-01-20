using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;

namespace FiapCloudGamesCatalog.Application.Dtos;

public class AuditGameUserCollectionResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public string Action { get; set; } = "";
    public string Collection { get; set; } = "";
    public string? Notes { get; set; }
    public DateTime DateCreated { get; set; }

    public static implicit operator AuditGameUserCollectionResponseDto?(AuditGameUserCollection? audit)
    {
        if (audit == null) return null;

        return new AuditGameUserCollectionResponseDto
        {
            Id = audit.Id,
            UserId = audit.UserId,
            GameId = audit.Game.Id,
            Action = audit.Action.ToString(),
            Collection = audit.Collection.ToString(),
            Notes = audit.Notes,
            DateCreated = audit.DateCreated
        };
    }
}