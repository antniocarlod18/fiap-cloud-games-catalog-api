using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class AddGamesToLibraryConsumer : IConsumer<OrderCompletedIntegrationEvent>
    {
        private readonly ILibraryService _libraryService;
        private readonly ILogger<AddGamesToLibraryConsumer> _logger;

        public AddGamesToLibraryConsumer(ILibraryService libraryService, ILogger<AddGamesToLibraryConsumer> logger) 
        {
            this._libraryService = libraryService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCompletedIntegrationEvent> context)
        {
            try
            {
                await _libraryService.AddGamesToLibrary(context.Message.UserId, context.Message.GameIds);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("AddGamesToLibraryConsumer: library not found for user {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddGamesToLibraryConsumer: error adding games to library for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
