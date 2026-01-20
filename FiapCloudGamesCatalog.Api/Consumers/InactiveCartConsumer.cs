using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class InactiveCartConsumer : IConsumer<UserLockedIntegrationEvent>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<InactiveCartConsumer> _logger;

        public InactiveCartConsumer(ICartService cartService, ILogger<InactiveCartConsumer> logger)
        {
            this._cartService = cartService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<UserLockedIntegrationEvent> context)
        {
            try
            {
                await _cartService.InactiveCartAsync(context.Message.UserId);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("InactiveCartConsumer: cart not found for user {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InactiveCartConsumer: error inactivating cart for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
