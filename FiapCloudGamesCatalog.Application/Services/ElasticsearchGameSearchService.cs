using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Clients.Elasticsearch.Security;
using FiapCloudGamesCatalog.Application.Caching;
using FiapCloudGamesCatalog.Application.Configurations;
using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Documents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FiapCloudGamesCatalog.Application.Services;

public class ElasticsearchGameSearchService : IGameSearchService
{
    private static readonly TimeSpan SearchCacheTtl = TimeSpan.FromMinutes(2);

    private readonly ElasticsearchClient _client;
    private readonly string _indexName;
    private readonly ICacheService _cache;
    private readonly ILogger<ElasticsearchGameSearchService> _logger;

    public ElasticsearchGameSearchService(
        ElasticsearchClient client,
        IOptions<GameSearchElasticsearchOptions> options,
        ICacheService cache, ILogger<ElasticsearchGameSearchService> logger)
    {
        _client = client;
        _indexName = options.Value.GamesSearchIndexName;
        _cache = cache;
        this._logger = logger;
    }

    public async Task<IReadOnlyList<GameSearchHitDto>> SearchAsync(
        string query,
        int size = 20,
        bool onlyAvailable = true,
        CancellationToken cancellationToken = default)
    {
        var take = Math.Clamp(size, 1, 100);
        var cacheKey = DistributedCacheKeys.Search(query, take, onlyAvailable);
        var cached = await _cache.GetAsync<List<GameSearchHitDto>>(cacheKey).ConfigureAwait(false);
        if (cached != null)
            return cached;

        var searchResponse = await _client.SearchAsync<GameSearchDocument>(
            s => s
                .Index(_indexName)
                .Size(take)
                .Query(q => q.Bool(b =>
                {
                    if (onlyAvailable)
                        b.Filter(f => f.Term(t => t.Field(new Field("available")).Value(true)));

                    b.Should(sh => sh.MultiMatch(mm => mm
                        .Query(query)
                        .Fields(new[]
                        {
                            "title^3",
                            "genre^2",
                            "description",
                            "developer",
                            "distributor"
                        })
                        .Fuzziness(new Fuzziness("AUTO"))
                        .Type(TextQueryType.BestFields)));

                    b.MinimumShouldMatch(1);
                }))
                .Sort(so => so.Score(sc => sc.Order(SortOrder.Desc))),
            cancellationToken).ConfigureAwait(false);

        if (!searchResponse.IsValidResponse)
        {
            _logger.LogWarning("Error search {searchResponse}", JsonSerializer.Serialize(searchResponse));
            return [];
        }

        var hits = searchResponse.Hits;
        if (hits == null)
            return [];

        var list = hits
            .Where(h => h.Source != null)
            .Select(h => new GameSearchHitDto
            {
                Id = Guid.Parse(h.Source!.Id),
                Title = h.Source.Title,
                Genre = h.Source.Genre,
                Description = h.Source.Description,
                Price = (decimal)h.Source.Price,
                Available = h.Source.Available,
                GamePlatforms = h.Source.GamePlatforms.ToList(),
                Developer = h.Source.Developer,
                Distributor = h.Source.Distributor,
                GameVersion = h.Source.GameVersion,
                Score = h.Score
            })
            .ToList();

        await _cache.SetAsync(cacheKey, list, SearchCacheTtl, cancellationToken).ConfigureAwait(false);
        return list;
    }
}
