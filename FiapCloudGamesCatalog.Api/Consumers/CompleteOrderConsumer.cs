using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Enums;
using FiapCloudGamesCatalog.Domain.Exceptions;
using MassTransit;

namespace FiapCloudGamesCatalog.Api.Consumers
{
    public class CompleteOrderConsumer : IConsumer<PaymentProcessedIntegrationEvent>
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<CompleteOrderConsumer> _logger;

        public CompleteOrderConsumer(IOrderService orderService, ILogger<CompleteOrderConsumer> logger) 
        {
            this._orderService = orderService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentProcessedIntegrationEvent> context)
        {
            try
            {
                if(context.Message.Status != PaymentStatusEnum.Approved)
                {
                    _logger.LogInformation("CompleteOrderConsumer: pagamento não aprovado para o pedido {OrderId}. Ignorando.", context.Message.OrderId);
                    return;
                }

                await _orderService.CompleteOrderAsync(context.Message.OrderId, context.Message.UserId);
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogInformation("CompleteOrderConsumer: order not found {OrderId}. Ignoring.", context.Message.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CompleteOrderConsumer: error completing order {OrderId}", context.Message.OrderId);
                throw;
            }
        }
    }
}
