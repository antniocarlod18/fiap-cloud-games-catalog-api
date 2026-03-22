using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.EventHandler;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderCreatedEventHandler> logger)
    {
        this._publishEndpoint = publishEndpoint;
        this._logger = logger;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("OrderCreatedEventHandler: Handling OrderCreatedDomainEvent for order {OrderId} user {UserId}", notification.OrderId, notification.UserId);

        await _publishEndpoint.Publish<OrderPlacedIntegrationEvent>(new()
        {
            OrderId = notification.OrderId,
            UserId = notification.UserId,
            GameIds = notification.GameIds.ToList(),
            Price = notification.Price
        });

        _logger.LogInformation("OrderCreatedEventHandler: Published OrderPlacedIntegrationEvent for order {OrderId}", notification.OrderId);

        return;
    }
}
