using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class InactiveLibraryConsumer : IConsumer<UserLockedIntegrationEvent>
    {
        private readonly ILibraryService _libraryService;
        private readonly ILogger<InactiveLibraryConsumer> _logger;

        public InactiveLibraryConsumer(ILibraryService libraryService, ILogger<InactiveLibraryConsumer> logger)
        {
            this._libraryService = libraryService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<UserLockedIntegrationEvent> context)
        {
            try
            {
                await _libraryService.InactiveLibraryAsync(context.Message.UserId);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("InactiveLibraryConsumer: library not found for user {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InactiveLibraryConsumer: error inactivating library for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
