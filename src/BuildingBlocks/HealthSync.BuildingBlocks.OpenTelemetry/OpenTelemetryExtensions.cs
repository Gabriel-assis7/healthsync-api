using HealthSync.BuildingBlocks.OpenTelemetry.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                        // Exclude health check endpoints from tracing
                        var basePath = context.Request.Path;

                        return !basePath.StartsWithSegments("/health")
                            && !basePath.StartsWithSegments("/metrics");
                    };
                });
                t.AddHttpClientInstrumentation();

                if (!string.IsNullOrWhiteSpace(options.ServiceName) && options.Sources?.Length > 0)
                {
                    t.AddSource(options.Sources);
                }

                //TODO: Add support for Jaeger and other exporters based on configuration
            });

        return services;
    }

    // TODO: Add support for metrics based on configuration
    // https://opentelemetry.io/docs/languages/dotnet/metrics/getting-started-aspnetcore/
    public static IServiceCollection AddOpenTelemetryMetrics(
        this IServiceCollection services,
        ObservabilityOptions options
    ) { }
}
