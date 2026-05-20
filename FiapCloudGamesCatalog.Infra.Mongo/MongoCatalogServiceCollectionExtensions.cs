using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FiapCloudGamesCatalog.Infra.Mongo;

public static class MongoCatalogServiceCollectionExtensions
{
    public static IServiceCollection AddMongoCatalog(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(
            new GuidSerializer(GuidRepresentation.Standard));

        services.Configure<MongoCatalogOptions>(configuration.GetSection(MongoCatalogOptions.SectionName));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoCatalogOptions>>().Value;
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new InvalidOperationException(
                    "MongoCatalog:ConnectionString is required (e.g. mongodb://localhost:27017).");
            }

            return new MongoClient(options.ConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoCatalogOptions>>().Value;
            var databaseName = string.IsNullOrWhiteSpace(options.DatabaseName)
                ? "catalog_mongo"
                : options.DatabaseName;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName);
        });

        services.AddSingleton<IStoredEventPersistence, MongoStoredEventPersistence>();
        services.AddScoped<IGameReviewRepository, GameReviewRepository>();
        services.AddHostedService<MongoCatalogInitializer>();

        return services;
    }
}
