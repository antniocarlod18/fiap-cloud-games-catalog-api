using FiapCloudGamesCatalog.Application.Services.Interfaces;

namespace FiapCloudGamesCatalog.Api.Endpoints;

public static class CartEndpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/{userId}/games/{gameId}/cart", AddGameToCart)
            .RequireAuthorization("SameUserOrAdmin"); 

        endpoints.MapDelete("/users/{userId}/games/{gameId}/cart", RemoveGameFromCart)
            .RequireAuthorization("SameUserOrAdmin");

        endpoints.MapGet("/users/{userId}/cart", GetAsync)
            .RequireAuthorization("SameUserOrAdmin");

        return endpoints;
    }
    public static async Task<IResult> GetAsync(Guid userId, ICartService service)
    {
        return Results.Ok(await service.GetCartByUserAsync(userId));
    }

    public static async Task<IResult> AddGameToCart(Guid userId, Guid gameId, ICartService service)
    {
        await service.AddGameToCart(userId, gameId);
        return Results.Ok();
    }

    public static async Task<IResult> RemoveGameFromCart(Guid userId, Guid gameId, ICartService service)
    {
        await service.RemoveGameFromCart(userId, gameId);
        return Results.Ok();
    }
}