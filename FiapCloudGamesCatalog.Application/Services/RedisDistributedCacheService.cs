using System.Text.Json;
using FiapCloudGamesCatalog.Application.Configurations;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace FiapCloudGamesCatalog.Application.Services;

public class RedisDistributedCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _multiplexer;
    private readonly IDatabase _db;
    private readonly ILogger<RedisDistributedCacheService> _logger;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public RedisDistributedCacheService(
        IConnectionMultiplexer multiplexer,
        IOptions<RedisConnectionOptions> options,
        ILogger<RedisDistributedCacheService> logger)
    {
        _multiplexer = multiplexer;
        _db = multiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var value = await _db.StringGetAsync(key).ConfigureAwait(false);
            if (!value.HasValue)
                return default;

            return JsonSerializer.Deserialize<T>((string)value!, _json);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis GET failed for key {CacheKey}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(value, _json);
            await _db.StringSetAsync(key, json, absoluteExpirationRelativeToNow).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis SET failed for key {CacheKey}", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var endpoint in _multiplexer.GetEndPoints())
            {
                var server = _multiplexer.GetServer(endpoint);
                if (!server.IsConnected || server.ServerType == ServerType.Sentinel)
                    continue;

                await foreach (var key in server.KeysAsync(_db.Database, $"{prefix}*").ConfigureAwait(false))
                {
                    await _db.KeyDeleteAsync(key).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis DELETE BY PREFIX failed for prefix {CachePrefix}", prefix);
        }
    }
}
