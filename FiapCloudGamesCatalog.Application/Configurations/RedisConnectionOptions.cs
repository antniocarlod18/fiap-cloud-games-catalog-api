namespace FiapCloudGamesCatalog.Application.Configurations;

public class RedisConnectionOptions
{
    public const string SectionName = "Redis";

    public string? ConnectionString { get; set; }
}
