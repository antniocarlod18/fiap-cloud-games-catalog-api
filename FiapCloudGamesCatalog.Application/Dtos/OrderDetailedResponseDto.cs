using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Enums;

namespace FiapCloudGamesCatalog.Application.Dtos;

public class OrderDetailedResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public IList<OrderGameItemResponseDto?> Games { get; set; } = [];
    public decimal Price { get; set; }
    public string Status { get; set; } = "";
    public DateTime DateCreated { get; set; }

    public static implicit operator OrderDetailedResponseDto?(Order? order)
    {
        if (order == null) return null;
        
        return new OrderDetailedResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            Games = order.Games.Select(x => (OrderGameItemResponseDto?)x).ToList(),
            Price = order.Price,
            Status = order.Status.ToString(),
            DateCreated = order.DateCreated
        };
    }
}