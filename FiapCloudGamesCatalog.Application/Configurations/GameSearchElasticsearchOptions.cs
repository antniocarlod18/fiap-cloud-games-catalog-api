namespace FiapCloudGamesCatalog.Application.Configurations;

public class GameSearchElasticsearchOptions
{
    public const string SectionName = "ElasticSearch";

    public string? Uri { get; set; }
    public string? ApiKey { get; set; }
    public string GamesSearchIndexName { get; set; } = "cataloggames";
}
