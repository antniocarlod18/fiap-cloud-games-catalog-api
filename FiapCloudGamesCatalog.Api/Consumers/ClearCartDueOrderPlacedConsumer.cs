using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class ClearCartDueOrderPlacedConsumer : IConsumer<OrderPlacedIntegrationEvent>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<ClearCartDueOrderPlacedConsumer> _logger;

        public ClearCartDueOrderPlacedConsumer(ICartService cartService, ILogger<ClearCartDueOrderPlacedConsumer> logger) 
        {
            this._cartService = cartService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
        {
            try
            {
                await _cartService.RemoveGamesFromCart(context.Message.UserId, context.Message.GameIds);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("ClearCartDueOrderPlacedConsumer: cart or games not found for user {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClearCartDueOrderPlacedConsumer: error clearing cart for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
