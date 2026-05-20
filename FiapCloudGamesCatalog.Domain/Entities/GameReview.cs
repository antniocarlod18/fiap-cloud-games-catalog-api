namespace FiapCloudGamesCatalog.Domain.Entities;

public class GameReview
{
    public GameReview()
    {
    }

    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
