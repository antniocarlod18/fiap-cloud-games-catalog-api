using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGamesCatalog.Api.Endpoints;

public static class GameEndpoints
{
    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/games", AddAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapGet("/games/{id}", GetAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        endpoints.MapGet("/games/title", GetByTitleAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        endpoints.MapGet("/games/genre", GetByGenreAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        endpoints.MapGet("/games/available", GetAllAvailableAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        endpoints.MapPut("/games/{id}", UpdateAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapGet("/games", GetAllAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        endpoints.MapDelete("/games/{id}", DeleteAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        return endpoints;
    }

    public static async Task<IResult> AddAsync(GameRequestDto gameRequestDto, IGameService gameService, IValidator<GameRequestDto> validator) 
    {
        var validationResult = await validator.ValidateAsync(gameRequestDto);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return Results.Created("/games", await gameService.AddAsync(gameRequestDto));
    }

    public static async Task<IResult> GetAsync(Guid id, IGameService gameService)
    {
        return Results.Ok(await gameService.GetAsync(id));
    }

    public static async Task<IResult> GetByTitleAsync([FromQuery]string title, IGameService gameService)
    {
        return Results.Ok(await gameService.GetByTitleAsync(title));
    }

    public static async Task<IResult> GetByGenreAsync([FromQuery]string genre, IGameService gameService)
    {
        return Results.Ok(await gameService.GetByGenreAsync(genre));
    }

    public static async Task<IResult> GetAllAvailableAsync(IGameService gameService)
    {
        return Results.Ok(await gameService.GetAllAvailableAsync());
    }

    public static async Task<IResult> UpdateAsync(Guid id, GameRequestDto gameRequestDto, IGameService gameService, IValidator<GameRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(gameRequestDto);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return Results.Ok(await gameService.UpdateAsync(id, gameRequestDto));
    }

    public static async Task<IResult> GetAllAsync(IGameService gameService)
    {
        return Results.Ok(await gameService.GetAllAsync());
    }

    public static async Task<IResult> DeleteAsync(Guid id, IGameService gameService)
    {
        await gameService.DeleteAsync(id);
        return Results.Ok();
    }
}
