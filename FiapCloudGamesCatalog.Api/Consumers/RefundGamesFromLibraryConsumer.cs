using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class RefundGamesFromLibraryConsumer : IConsumer<OrderRefundedIntegrationEvent>
    {
        private readonly ILibraryService _libraryService;
        private readonly ILogger<RefundGamesFromLibraryConsumer> _logger;

        public RefundGamesFromLibraryConsumer(ILibraryService libraryService, ILogger<RefundGamesFromLibraryConsumer> logger) 
        {
            this._libraryService = libraryService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderRefundedIntegrationEvent> context)
        {
            try
            {
                await _libraryService.RemoveGamesFromLibrary(context.Message.UserId, context.Message.GameIds);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("RefundGamesFromLibraryConsumer: library or games not found for user {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RefundGamesFromLibraryConsumer: error removing games from library for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
