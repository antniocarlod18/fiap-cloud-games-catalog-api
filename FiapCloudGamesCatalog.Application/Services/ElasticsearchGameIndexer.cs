using Elastic.Clients.Elasticsearch;
using FiapCloudGamesCatalog.Application.Configurations;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Documents;
using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.Extensions.Options;

namespace FiapCloudGamesCatalog.Application.Services;

public class ElasticsearchGameIndexer : IGameIndexer
{
    private readonly ElasticsearchClient _client;
    private readonly string _indexName;

    public ElasticsearchGameIndexer(ElasticsearchClient client, IOptions<GameSearchElasticsearchOptions> options)
    {
        _client = client;
        _indexName = options.Value.GamesSearchIndexName;
    }

    public async Task IndexAsync(Game game, CancellationToken cancellationToken = default)
    {
        await EnsureIndexAsync(cancellationToken);

        var doc = ToDocument(game);

        var response = await _client.IndexAsync(doc, i => i.Index(_indexName).Id(doc.Id).Refresh(Refresh.WaitFor), cancellationToken);

        if (!response.IsValidResponse)
        {
            throw new Exception(response.DebugInformation);
        }       
    }

    private async Task EnsureIndexAsync(CancellationToken cancellationToken)
    {
        var exists = await _client.Indices.ExistsAsync(_indexName, cancellationToken);
        if (!exists.Exists)
        {
            var createResponse = await _client.Indices.CreateAsync(
                _indexName,
                c => c.Mappings(m => m.Properties<GameSearchDocument>(p => p
                    .Keyword(d => d.Id)
                    .Text(d => d.Title)
                    .Text(d => d.Genre)
                    .Text(d => d.Description)
                    .DoubleNumber(d => d.Price)
                    .Boolean(d => d.Available)
                    .Keyword(d => d.GamePlatforms)
                    .Text(d => d.Developer)
                    .Text(d => d.Distributor)
                    .Text(d => d.GameVersion))),
                cancellationToken);
                
            if (!createResponse.IsValidResponse)
            {
                throw new Exception(createResponse.DebugInformation);
            }
        }
    }

    private static GameSearchDocument ToDocument(Game game) => new()
    {
        Id = game.Id.ToString("D"),
        Title = game.Title,
        Genre = game.Genre,
        Description = game.Description,
        Price = (double)game.Price,
        Available = game.Available,
        GamePlatforms = game.GamePlatforms.Select(gp => gp.ToString()).ToList(),
        Developer = game.Developer,
        Distributor = game.Distributor,
        GameVersion = game.GameVersion
    };
}
