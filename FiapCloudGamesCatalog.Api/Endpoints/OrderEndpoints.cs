using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FluentValidation;
using System.Security.Claims;

namespace FiapCloudGamesCatalog.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/{userId}/orders", AddAsync)
            .RequireAuthorization("SameUserOrAdmin");

        endpoints.MapGet("/orders/{id}", GetAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        endpoints.MapGet("/users/{userId}/orders", GetByUserAsync)
            .RequireAuthorization("SameUserOrAdmin");

        endpoints.MapPost("/orders/{id}/cancel", CancelAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        endpoints.MapPost("/orders/{id}/complete", CompleteAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        endpoints.MapPost("/orders/{id}/refund", RefundAsync)
            .RequireAuthorization(policy => policy.RequireRole("Admin", "User"));

        return endpoints;
    }

    public static async Task<IResult> AddAsync(Guid userId, IOrderService service)
    {
        var order = await service.AddAsync(userId);
        return Results.Created($"/orders/{order.Id}", order);
    }

    public static async Task<IResult> GetAsync(Guid id, IOrderService service, HttpContext context)
    {
        Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

        return Results.Ok(await service.GetAsync(id, userId, role));
    }

    public static async Task<IResult> GetByUserAsync(Guid userId, IOrderService service)
    {
        return Results.Ok(await service.GetByUserAsync(userId));
    }

    public static async Task<IResult> CancelAsync(Guid id, IOrderService service, HttpContext context)
    {
        Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

        var order = await service.CancelOrderAsync(id, userId, role);
        return Results.Created($"/orders/{order.Id}", order);
    }

    public static async Task<IResult> CompleteAsync(Guid id, IOrderService service, HttpContext context)
    {
        Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

        var order = await service.CompleteOrderAsync(id, userId, role);
        return Results.Created($"/orders/{order.Id}", order);
    }

    public static async Task<IResult> RefundAsync(Guid id, IOrderService service, HttpContext context)
    {
        Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

        var order = await service.RefundOrderAsync(id, userId, role);
        return Results.Created($"/orders/{order.Id}", order);
    }
}