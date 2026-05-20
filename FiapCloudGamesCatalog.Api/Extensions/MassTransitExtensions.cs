using FiapCloudGamesCatalog.Api.Consumers;
using FiapCloudGamesCatalog.Api.Filters;
using MassTransit;

namespace FiapCloudGamesCatalog.Api.Extensions
{
    public static class MassTransitExtensions
    {
        public static WebApplicationBuilder AddMassTransitConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: builder.Environment.EnvironmentName, includeNamespace: false));

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.UseSendFilter(typeof(TracingSendFilter<>), context);
                    cfg.UsePublishFilter(typeof(TracingPublishFilter<>), context);

                    cfg.UseConsumeFilter(typeof(TracingConsumeFilter<>), context);

                    var connectionString = builder.Configuration["AzureServiceBus:ConnectionString"];
                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidOperationException(
                            "Configure AzureServiceBus:ConnectionString with the Service Bus namespace connection string (Azure Portal → namespace → Shared access policies).");
                    }

                    cfg.Host(connectionString);

                    cfg.UseMessageRetry(r => r.Immediate(2));
                    cfg.ConfigureEndpoints(context);
                });

                x.AddConsumer<AddGamesToLibraryConsumer>();
                x.AddConsumer<ClearCartDueOrderPlacedConsumer>();
                x.AddConsumer<RefundGamesFromLibraryConsumer>();
                x.AddConsumer<ReturnGamesToCartDueOrderCanceledConsumer>();
                x.AddConsumer<CompleteOrderConsumer>();
                x.AddConsumer<CreateCartConsumer>();
                x.AddConsumer<CreateLibraryConsumer>();
                x.AddConsumer<InactiveCartConsumer>();
                x.AddConsumer<InactiveLibraryConsumer>();                
            });

            return builder;
        }
    }
}
