using Elastic.Apm.SerilogEnricher;
using Elastic.Channels;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Serilog;

namespace FiapCloudGamesCatalog.Api.Extensions;

public static class ElasticExtensions
{
    public static WebApplicationBuilder AddElasticConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithCorrelationId()
                .Enrich.WithMachineName()
                .Enrich.WithElasticApmCorrelationInfo()
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .WriteTo.Console() 
                .WriteTo.Elasticsearch([new Uri(builder.Configuration["ElasticSearch:Uri"])], opts =>
                {
                    opts.DataStream = new DataStreamName("logs", builder.Configuration["ElasticSearch:IndexName"], context.HostingEnvironment.EnvironmentName);
                    opts.BootstrapMethod = BootstrapMethod.Failure;
                    opts.TextFormatting = new EcsTextFormatterConfiguration<LogEventEcsDocument>();
                    opts.ConfigureChannel = channelOpts =>
                    {
                        channelOpts.BufferOptions = new BufferOptions();
                    };
                }, transport =>
                {
                    transport.Authentication(new ApiKey(builder.Configuration["ElasticSearch:ApiKey"]));
                });
        });

        builder.Services.AddAllElasticApm();

        return builder;
    }
}
