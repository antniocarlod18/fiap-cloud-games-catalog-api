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

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseSendFilter(typeof(TracingSendFilter<>), context);
                    cfg.UsePublishFilter(typeof(TracingPublishFilter<>), context);

                    cfg.UseConsumeFilter(typeof(TracingConsumeFilter<>), context);

                    cfg.Host(builder.Configuration["RabbitMQ:Host"], builder.Configuration["RabbitMQ:VirtualHost"], h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:UserName"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

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
