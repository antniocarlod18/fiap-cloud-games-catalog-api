namespace FiapCloudGamesCatalog.Domain.Documents;

public class GameSearchDocument
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Genre { get; init; }
    public string? Description { get; init; }
    public double Price { get; init; }
    public bool Available { get; init; }
    public IReadOnlyList<string> GamePlatforms { get; init; } = [];
    public string? Developer { get; init; }
    public string? Distributor { get; init; }
    public string? GameVersion { get; init; }
}
