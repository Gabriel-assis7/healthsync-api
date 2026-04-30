using HealthSync.BuildingBlocks.OpenTelemetry.Configuration;
using HealthSync.BuildingBlocks.OpenTelemetry.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;

namespace HealthSync.BuildingBlocks.OpenTelemetry.Extensions;

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

        builder
            .Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource
                    .AddService(
                        serviceName: options.ServiceName,
                        serviceVersion: options.ServiceVersion
                    )
                    .AddAttributes(
                        new Dictionary<string, object>
                        {
                            ["deployment.environment"] = options.Environment,
                            ["service.namespace"] = options.ServiceNamespace ?? string.Empty,
                            ["service.instance.id"] =
                                Environment.GetEnvironmentVariable("HOSTNAME")
                                ?? Guid.NewGuid().ToString(),
                            ["service.host.name"] =
                                Environment.GetEnvironmentVariable("HOSTNAME")
                                ?? Environment.MachineName,
                            ["service.host.type"] =
                                Environment.GetEnvironmentVariable("HOST_TYPE") ?? string.Empty,
                            ["service.process.id"] = Environment.ProcessId,
                        }
                    )
                    .AddTelemetrySdk()
                    .AddEnvironmentVariableDetector();
            })
            .WithTracing(t => TracingConfiguration.ConfigureTracing(t, options))
            .WithMetrics(m => MetricsConfiguration.ConfigureMetrics(m, options));

        return builder;
    }
}
