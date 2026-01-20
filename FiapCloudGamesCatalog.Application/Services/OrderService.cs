using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger)
    {
        this._unitOfWork = unitOfWork;
        this._logger = logger;
    }

    public async Task<OrderResponseDto?> AddAsync(Guid idUser)
    {
        _logger.LogInformation("Creating order for user {UserId}", idUser);

        var cart = await _unitOfWork.CartsRepo.GetByUserAsync(idUser);
        if (cart == null || !cart.Games.Any())
        {
            _logger.LogWarning("Cart not found or empty for user {UserId}", idUser);
            throw new ResourceNotFoundException(nameof(Cart));
        }

        var orders = await _unitOfWork.OrdersRepo.GetByUserAsync(idUser);

        if (orders.Any(o => o.Status == Domain.Enums.OrderStatusEnum.WaitingForPayment))
        {
            _logger.LogWarning("User {UserId} already has an open order", idUser);
            throw new UserAlreadyHasAnOpenOrderException();
        }

        var order = new Order(cart.UserId, cart.Games);

        await _unitOfWork.OrdersRepo.AddAsync(order);
        await _unitOfWork.Commit();

        _logger.LogInformation("Created order {OrderId} for user {UserId}", order.Id, idUser);

        return order;
    }

    public async Task<OrderDetailedResponseDto?> GetAsync(Guid orderId, Guid idUser, string role)
    {
        var order = await _unitOfWork.OrdersRepo.GetDetailedByIdAsync(orderId);
        
        if (order == null || (order.UserId != idUser && role != "Admin"))
            throw new ResourceNotFoundException(nameof(Order));

        return order;
    }

    public async Task<IList<OrderResponseDto?>> GetByUserAsync(Guid idUser)
    {
        var orders = await _unitOfWork.OrdersRepo.GetByUserAsync(idUser);
        if (orders == null || !orders.Any())
        {
            _logger.LogInformation("No orders found for user {UserId}", idUser);
            return [];
        }

        _logger.LogInformation("Retrieved {Count} orders for user {UserId}", orders.Count, idUser);
        return orders.Select(x => (OrderResponseDto?)x).ToList();
    }

    public async Task<OrderDetailedResponseDto?> CancelOrderAsync(Guid orderId, Guid idUser, string role)
    {
        var order = await _unitOfWork.OrdersRepo.GetDetailedByIdAsync(orderId);
        if (order == null || (order.UserId != idUser && role != "Admin"))
        {
            _logger.LogWarning("Order {OrderId} not found or access denied for user {UserId}", orderId, idUser);
            throw new ResourceNotFoundException(nameof(Order));
        }

        order.CancelOrder();

        _unitOfWork.OrdersRepo.Update(order);
        await _unitOfWork.Commit();

        _logger.LogInformation("Canceled order {OrderId} for user {UserId}", orderId, idUser);
        return order;
    }

    public async Task<OrderDetailedResponseDto?> CompleteOrderAsync(Guid orderId, Guid idUser, string? role = null)
    {
        var order = await _unitOfWork.OrdersRepo.GetDetailedByIdAsync(orderId);
        if (order == null || (order.UserId != idUser && role != "Admin"))
        {
            _logger.LogWarning("Order {OrderId} not found or access denied for user {UserId}", orderId, idUser);
            throw new ResourceNotFoundException(nameof(Order));
        }

        order.CompletedOrder();

        _unitOfWork.OrdersRepo.Update(order);
        await _unitOfWork.Commit();

        _logger.LogInformation("Completed order {OrderId} for user {UserId}", orderId, idUser);
        return order;
    }

    public async Task<OrderDetailedResponseDto?> RefundOrderAsync(Guid orderId, Guid idUser, string role)
    {
        var order = await _unitOfWork.OrdersRepo.GetDetailedByIdAsync(orderId);
        if (order == null || (order.UserId != idUser && role != "Admin"))
        {
            _logger.LogWarning("Order {OrderId} not found or access denied for user {UserId}", orderId, idUser);
            throw new ResourceNotFoundException(nameof(Order));
        }

        order.RefundOrder();

        _unitOfWork.OrdersRepo.Update(order);
        await _unitOfWork.Commit();

        _logger.LogInformation("Refunded order {OrderId} for user {UserId}", orderId, idUser);
        return order;
    }
}