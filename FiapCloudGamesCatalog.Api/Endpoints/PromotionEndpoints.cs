using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FluentValidation;

namespace FiapCloudGamesCatalog.Api.Endpoints;

public static class PromotionEndpoints
{
    public static IEndpointRouteBuilder MapPromotionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/promotions", AddAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin")); 

        endpoints.MapGet("/promotions/{id}", GetAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapGet("/promotions/active", GetActiveAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapGet("/promotions", GetAllAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapDelete("/promotions/{id}", DeleteAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapPost("/promotions/{id}/games/{gameId}", AddGameToPromotionAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapDelete("/promotions/{id}/games/{gameId}", RemoveGameToPromotionAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        return endpoints;
    }

    public static async Task<IResult> AddAsync(PromotionRequestDto dto, IPromotionService service, IValidator<PromotionRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var created = await service.AddAsync(dto);
        return Results.Created($"/promotions/{created?.Id}", created);
    }

    public static async Task<IResult> GetAsync(Guid id, IPromotionService service)
    {
        return Results.Ok(await service.GetAsync(id));
    }

    public static async Task<IResult> GetActiveAsync(IPromotionService service)
    {
        return Results.Ok(await service.GetActiveAsync());
    }

    public static async Task<IResult> GetAllAsync(IPromotionService service)
    {
        return Results.Ok(await service.GetAllAsync());
    }

    public static async Task<IResult> DeleteAsync(Guid id, IPromotionService service)
    {
        await service.DeleteAsync(id);
        return Results.Ok();
    }

    public static async Task<IResult> AddGameToPromotionAsync(Guid id, Guid gameId, IPromotionService service)
    {
        return Results.Ok(await service.AddGameToPromotionAsync(id, gameId));
    }

    public static async Task<IResult> RemoveGameToPromotionAsync(Guid id, Guid gameId, IPromotionService service)
    {
        return Results.Ok(await service.RemoveGameToPromotionAsync(id, gameId));
    }
}