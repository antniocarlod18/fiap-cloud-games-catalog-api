namespace FiapCloudGamesCatalog.Application.Dtos;

public class GameReviewPageResponseDto
{
    public IReadOnlyList<GameReviewResponseDto> Items { get; set; } = [];
    public long TotalCount { get; set; }
}
