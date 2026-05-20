using FiapCloudGamesCatalog.Application.Configurations;
using FiapCloudGamesCatalog.Application.Services;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using StackExchange.Redis;

namespace FiapCloudGamesCatalog.Api.Extensions;

public static class RedisExtensions
{
    public static WebApplicationBuilder AddRedisDistributedCache(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RedisConnectionOptions>(
            builder.Configuration.GetSection(RedisConnectionOptions.SectionName));

        var redisSection = builder.Configuration.GetSection(RedisConnectionOptions.SectionName);
        var connectionString = redisSection["ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Configure Redis:ConnectionString (e.g. localhost:6379,abortConnect=false).");
        }

        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(connectionString));

        builder.Services.AddSingleton<ICacheService, RedisDistributedCacheService>();

        return builder;
    }
}
