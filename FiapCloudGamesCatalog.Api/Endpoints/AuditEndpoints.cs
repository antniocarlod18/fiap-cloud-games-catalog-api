using FiapCloudGamesCatalog.Application.Services.Interfaces;

namespace FiapCloudGamesCatalog.Api.Endpoints;

public static class AuditEndpoints
{
    public static IEndpointRouteBuilder MapAuditEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/audits/users/{userId}/collections", GetAuditsByUserAsync)
            .RequireAuthorization("SameUserOrAdmin");

        endpoints.MapGet("/audits/games/{gameId}/collections", GetAuditsByGameAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapGet("/audits/games/{gameId}/prices", GetAuditsPriceByGameAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        return endpoints;
    } 

    public static async Task<IResult> GetAuditsByUserAsync(Guid userId, string? collection, IAuditService service)
    {
        var list = await service.GetByUserAsync(userId, collection);
        return Results.Ok(list);
    }

    public static async Task<IResult> GetAuditsByGameAsync(Guid gameId, string? collection, IAuditService service)
    {
        var list = await service.GetByGameAsync(gameId, collection);
        return Results.Ok(list);
    }

    public static async Task<IResult> GetAuditsPriceByGameAsync(Guid gameId, IAuditService service)
    {
        var list = await service.GetByGameAsync(gameId);
        return Results.Ok(list);
    }
}