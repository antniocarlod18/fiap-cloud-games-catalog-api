using System.Security.Cryptography;
using System.Text;

namespace FiapCloudGamesCatalog.Application.Caching;

public static class DistributedCacheKeys
{
    public const string GamesPrefix = "catalog:games:";
    public const string SearchPrefix = "catalog:search:";
    public const string PromotionsPrefix = "catalog:promotions:";

    public static string GameById(Guid id) => $"{GamesPrefix}byid:{id:D}";

    public static string GamesAll() => $"{GamesPrefix}all";

    public static string GamesAvailable() => $"{GamesPrefix}available";

    public static string GamesByGenre(string genre)
    {
        var g = (genre ?? string.Empty).Trim().ToLowerInvariant();
        return $"{GamesPrefix}genre:{g}";
    }

    public static string GameByTitle(string title)
    {
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(title ?? string.Empty)));
        return $"{GamesPrefix}title:{hash}";
    }

    public static string Search(string query, int size, bool onlyAvailable)
    {
        var payload = $"{query}\u001e{size}\u001e{onlyAvailable}";
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload)));
        return $"{SearchPrefix}{hash}";
    }

    public static string PromotionsAll() => $"{PromotionsPrefix}all";

    public static string PromotionsActive() => $"{PromotionsPrefix}active";
}
