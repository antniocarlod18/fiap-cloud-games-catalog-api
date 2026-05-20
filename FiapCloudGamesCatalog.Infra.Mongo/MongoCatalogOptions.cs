namespace FiapCloudGamesCatalog.Infra.Mongo;

public class MongoCatalogOptions
{
    public const string SectionName = "MongoCatalog";

    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = "catalog_mongo";
}
