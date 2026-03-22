using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.EventHandler;

public class OrderCanceledEventHandler : INotificationHandler<OrderCanceledDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OrderCanceledEventHandler> _logger;

    public OrderCanceledEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderCanceledEventHandler> logger)
    {
        this._publishEndpoint = publishEndpoint;
        this._logger = logger;
    }
    

    public async Task Handle(OrderCanceledDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("OrderCanceledEventHandler: Handling OrderCanceledDomainEvent for order {OrderId}", notification.OrderId);

        await _publishEndpoint.Publish<OrderCanceledIntegrationEvent>(new()
        {
            OrderId = notification.OrderId,
            UserId = notification.UserId,
            GameIds = notification.GameIds.ToList()
        });

        _logger?.LogInformation("OrderCanceledEventHandler: Published OrderCompletedIntegrationEvent for order {OrderId}", notification.OrderId);

        return;
    }
}
