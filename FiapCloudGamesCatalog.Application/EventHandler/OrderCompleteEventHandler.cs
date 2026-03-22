using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Application.EventHandler
{
    public class OrderCompleteEventHandler : INotificationHandler<OrderCompleteDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderCompleteEventHandler> _logger;

        public OrderCompleteEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderCompleteEventHandler> logger)
        {
            this._publishEndpoint = publishEndpoint;
            this._logger = logger;
        }

        public async Task Handle(OrderCompleteDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("OrderCompleteEventHandler: Handling OrderCompleteDomainEvent for order {OrderId}", notification.OrderId);

            await _publishEndpoint.Publish<OrderCompletedIntegrationEvent>(new()
            {
                OrderId = notification.OrderId,
                UserId = notification.UserId,
                GameIds = notification.GameIds.ToList()
            });

            _logger?.LogInformation("OrderCompleteEventHandler: Published OrderCompletedIntegrationEvent for order {OrderId}", notification.OrderId);

            return;
        }
    }
}
