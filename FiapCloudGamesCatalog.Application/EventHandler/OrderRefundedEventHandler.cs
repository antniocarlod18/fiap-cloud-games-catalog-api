using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.EventHandler;

public class OrderRefundedEventHandler : INotificationHandler<OrderRefundedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OrderRefundedEventHandler> _logger;

    public OrderRefundedEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderRefundedEventHandler> logger)
    {
        this._publishEndpoint = publishEndpoint;
        this._logger = logger;
    }
    

    public async Task Handle(OrderRefundedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("OrderRefundedEventHandler: Handling OrderRefundedDomainEvent for order {OrderId}", notification.OrderId);

        await _publishEndpoint.Publish<OrderRefundedIntegrationEvent>(new()
        {
            OrderId = notification.OrderId,
            UserId = notification.UserId,
            GameIds = notification.Games.Select(x => x.Game.Id).ToList()
        });

        _logger?.LogInformation("OrderRefundedEventHandler: Published OrderRefundedIntegrationEvent for order {OrderId}", notification.OrderId);

        return;
    }
}
