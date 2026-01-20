using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class ReturnGamesToCartDueOrderCanceledConsumer : IConsumer<OrderCanceledIntegrationEvent>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<ReturnGamesToCartDueOrderCanceledConsumer> _logger;

        public ReturnGamesToCartDueOrderCanceledConsumer(ICartService cartService, ILogger<ReturnGamesToCartDueOrderCanceledConsumer> logger) 
        {
            this._cartService = cartService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCanceledIntegrationEvent> context)
        {
            try
            {
                await _cartService.AddGamesToCart(context.Message.UserId, context.Message.GameIds);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("ReturnGamesToCartDueOrderCanceledConsumer: cart or games not found for user {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReturnGamesToCartDueOrderCanceledConsumer: error returning games to cart for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
