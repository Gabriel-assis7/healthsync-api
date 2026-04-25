using HealthSync.BuildingBlocks.OpenTelemetry.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace HealthSync.BuildingBlocks.OpenTelemetry;

public static class AddObservabilityExtensions
{
    public static IHostApplicationBuilder AddObservability(
        this IHostApplicationBuilder builder,
        Action<ObservabilityOptions>? configurator = null
    )
    {
        builder
            .Services.AddOptions<ObservabilityOptions>()
            .Bind(builder.Configuration.GetSection("Observability"))
            .Validate(
                config => !string.IsNullOrWhiteSpace(config.ServiceName),
                "Observability:ServiceName is required"
            )
            .ValidateOnStart();

        if (configurator != null)
            builder.Services.Configure(configurator);

        var options =
            builder.Configuration.GetSection("Observability").Get<ObservabilityOptions>()
            ?? new ObservabilityOptions();

        configurator?.Invoke(options);

        if (!options.Enabled)
            return builder;

        builder.Services.AddOpenTelemetryTracing(options);
        builder.Services.AddOpenTelemetryMetrics(options);

        return builder;
    }

    // https://opentelemetry.io/docs/languages/dotnet/traces/getting-started-aspnetcore/
    public static IServiceCollection AddOpenTelemetryTracing(
        this IServiceCollection services,
        ObservabilityOptions options
    )
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(
                    serviceName: options.ServiceName,
                    serviceVersion: options.ServiceVersion
                );
            })
            .WithTracing(t =>
            {
                t.AddAspNetCoreInstrumentation(opt =>
                {
                    opt.Filter = context =>
                    {
                        // Exclude  endpoints from tracing
                        var basePath = context.Request.Path;

                        return !basePath.StartsWithSegments("/health")
                            && !basePath.StartsWithSegments("/metrics");
                    };
                });
                t.AddHttpClientInstrumentation();

                if (options.Sources?.Length > 0)
                {
                    t.AddSource(options.Sources);
                }

                switch (options)
                {
                    case { ExportToJaeger: true, Jaeger: not null }:
                        t.AddOtlpExporter(opt =>
                        {
                            var uri = new Uri(options.Jaeger.Endpoint);
                            opt.Endpoint = uri;
                            opt.Protocol = options.Jaeger.Protocol?.ToLower() switch
                            {
                                "grpc" => OtlpExportProtocol.Grpc,
                                "http" or "http/protobuf" => OtlpExportProtocol.HttpProtobuf,
                                _ => uri.Port == 4318
                                    ? OtlpExportProtocol.HttpProtobuf
                                    : OtlpExportProtocol.Grpc,
                            };
                            if (!string.IsNullOrWhiteSpace(options.Jaeger.Headers))
                            {
                                opt.Headers = options.Jaeger.Headers;
                            }
                        });
                        break;

                    case { OtlpEndpoint: not null }:
                        t.AddOtlpExporter(opt =>
                        {
                            var uri = new Uri(options.OtlpEndpoint);
                            opt.Endpoint = uri;

                            opt.Protocol = uri.Port switch
                            {
                                4317 => OtlpExportProtocol.Grpc,
                                4318 => OtlpExportProtocol.HttpProtobuf,
                                _ => OtlpExportProtocol.Grpc,
                            };
                        });
                        break;
                }
                ;
            });

        return services;
    }

    // TODO: Add support for metrics based on configuration
    // https://opentelemetry.io/docs/languages/dotnet/metrics/getting-started-aspnetcore/
    public static IServiceCollection AddOpenTelemetryMetrics(
        this IServiceCollection services,
        ObservabilityOptions options
    )
    {
        
    }
}
