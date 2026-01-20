using FiapCloudGamesCatalog.Application.Dtos;

namespace FiapCloudGamesCatalog.Application.Services.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto?> AddAsync(Guid idUser);
    Task<OrderDetailedResponseDto?> GetAsync(Guid orderId, Guid idUser, string role);
    Task<IList<OrderResponseDto?>> GetByUserAsync(Guid idUser);
    Task<OrderDetailedResponseDto?> CancelOrderAsync(Guid orderId, Guid idUser, string role);
    Task<OrderDetailedResponseDto?> CompleteOrderAsync(Guid orderId, Guid idUser, string? role = null);
    Task<OrderDetailedResponseDto?> RefundOrderAsync(Guid orderId, Guid idUser, string role);
}