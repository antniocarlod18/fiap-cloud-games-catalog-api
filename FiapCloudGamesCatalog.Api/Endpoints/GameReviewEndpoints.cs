using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGamesCatalog.Api.Endpoints;

public static class GameReviewEndpoints
{
    public static IEndpointRouteBuilder MapGameReviewEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/{userId}/games/{gameId}/reviews", CreateReviewAsync)
            .RequireAuthorization("SameUserOrAdmin");

        endpoints.MapGet("/games/{gameId}/reviews", GetReviewsByGameAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        return endpoints;
    }

    public static async Task<IResult> CreateReviewAsync(
        Guid userId,
        Guid gameId,
        GameReviewRequestDto body,
        IGameReviewService gameReviewService,
        IValidator<GameReviewRequestDto> validator)
    {
        var validation = await validator.ValidateAsync(body);
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var created = await gameReviewService.CreateAsync(userId, gameId, body);
        return Results.Created($"/games/{gameId}/reviews/{created.Id}", created);
    }

    public static async Task<IResult> GetReviewsByGameAsync(
        Guid gameId,
        [FromQuery] int skip,
        [FromQuery] int take,
        IGameReviewService gameReviewService)
    {
        var page = await gameReviewService.GetByGameAsync(gameId, skip, take > 0 ? take : 20);
        return Results.Ok(page);
    }
}
