using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class CreateCartConsumer : IConsumer<UserUnlockedIntegrationEvent>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CreateCartConsumer> _logger;

        public CreateCartConsumer(ICartService cartService, ILogger<CreateCartConsumer> logger)
        {
            this._cartService = cartService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<UserUnlockedIntegrationEvent> context)
        {
            try
            {
                await _cartService.CreateCartAsync(context.Message.UserId);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("CreateCartConsumer: resource not found when creating cart for {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateCartConsumer: error creating cart for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
