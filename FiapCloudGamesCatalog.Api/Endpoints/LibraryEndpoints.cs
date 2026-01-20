using FiapCloudGamesCatalog.Application.Services.Interfaces;

namespace FiapCloudGamesCatalog.Api.Endpoints;

public static class LibraryEndpoints
{
    public static IEndpointRouteBuilder MapLibraryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/users/{userId}/library", GetAsync)
            .RequireAuthorization("SameUserOrAdmin");

        return endpoints;
    }
    public static async Task<IResult> GetAsync(Guid userId, ILibraryService service)
    {
        return Results.Ok(await service.GetLibraryByUserAsync(userId));
    }
}