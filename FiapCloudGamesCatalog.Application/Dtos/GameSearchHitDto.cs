namespace FiapCloudGamesCatalog.Application.Dtos;

public class GameSearchHitDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool Available { get; set; }
    public IList<string> GamePlatforms { get; set; } = [];
    public string? Developer { get; set; }
    public string? Distributor { get; set; }
    public string? GameVersion { get; set; }
    public double? Score { get; set; }
}
