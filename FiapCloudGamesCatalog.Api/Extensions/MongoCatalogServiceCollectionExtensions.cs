using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Infra.Data;
using FiapCloudGamesCatalog.Infra.Data.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FiapCloudGamesCatalog.Api.Extensions;

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

        services.AddSingleton<IStoredEventPersistence, StoredEventPersistence>();
        services.AddScoped<IGameReviewRepository, GameReviewRepository>();
        services.AddHostedService<MongoCatalogInitializer>();

        return services;
    }
}

public class MongoCatalogOptions
{
    public const string SectionName = "MongoCatalog";

    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = "catalog_mongo";
}

public static class MongoCollectionNames
{
    public const string StoredEvents = "stored_events";
    public const string GameReviews = "game_reviews";
}
