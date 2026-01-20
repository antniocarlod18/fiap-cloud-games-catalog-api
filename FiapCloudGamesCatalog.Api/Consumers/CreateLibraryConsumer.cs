using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class CreateLibraryConsumer : IConsumer<UserUnlockedIntegrationEvent>
    {
        private readonly ILibraryService _libraryService;
        private readonly ILogger<CreateLibraryConsumer> _logger;

        public CreateLibraryConsumer(ILibraryService libraryService, ILogger<CreateLibraryConsumer> logger)
        {
            this._libraryService = libraryService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<UserUnlockedIntegrationEvent> context)
        {
            try
            {
                await _libraryService.CreateLibraryAsync(context.Message.UserId);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("CreateLibraryConsumer: resource not found when creating library for {UserId}. Ignoring.", context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateLibraryConsumer: error creating library for user {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
