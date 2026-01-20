using FiapCloudGamesCatalog.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class AuditGameUserCollection : EntityBase
{
    public required Guid UserId { get; set; }
    public required Game Game { get; set; }
    public required AuditGameUserActionEnum Action { get; set; }
    public required AuditGameUserCollectionEnum Collection { get; set; }
    public string? Notes { get; set; }

    [SetsRequiredMembers]
    public AuditGameUserCollection(Guid userId, Game game, AuditGameUserActionEnum action, AuditGameUserCollectionEnum collection, string? notes = null) : base()
    {
        UserId = userId;
        Game = game;
        Action = action;
        Collection = collection;
        Notes = notes;
    }

    [SetsRequiredMembers]
    protected AuditGameUserCollection()
    {
    }
}
