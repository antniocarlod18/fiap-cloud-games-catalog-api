namespace FiapCloudGamesCatalog.Application.Dtos;

public class PromotionRequestDto
{
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IList<Guid> GameIds { get; set; } = [];
}