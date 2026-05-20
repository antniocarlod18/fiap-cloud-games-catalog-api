using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using FiapCloudGamesCatalog.Application.Configurations;
using FiapCloudGamesCatalog.Application.Services;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FiapCloudGamesCatalog.Api.Extensions;

public static class ElasticsearchGameSearchExtensions
{
    public static WebApplicationBuilder AddGameSearchElasticsearch(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<GameSearchElasticsearchOptions>(
            builder.Configuration.GetSection(GameSearchElasticsearchOptions.SectionName));

        builder.Services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<GameSearchElasticsearchOptions>>().Value;
            if (string.IsNullOrWhiteSpace(options.Uri))
            {
                throw new InvalidOperationException(
                    "ElasticSearch:Uri is required for game search indexing (same cluster as Serilog if applicable).");
            }

            if (string.IsNullOrWhiteSpace(options.ApiKey))
            {
                throw new InvalidOperationException(
                    "ElasticSearch:ApiKey is required for game search indexing.");
            }

            var settings = new ElasticsearchClientSettings(new Uri(options.Uri))
                .Authentication(new ApiKey(options.ApiKey));

            return new ElasticsearchClient(settings);
        });

        builder.Services.AddScoped<IGameIndexer, ElasticsearchGameIndexer>();
        builder.Services.AddScoped<IGameSearchService, ElasticsearchGameSearchService>();

        return builder;
    }
}
