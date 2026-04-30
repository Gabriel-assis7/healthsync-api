using System.Diagnostics;
using System.Diagnostics.Metrics;
using HealthSync.BuildingBlocks.OpenTelemetry.Settings;
using OpenTelemetry.Metrics;

namespace HealthSync.BuildingBlocks.OpenTelemetry.Configuration;

public static class MetricsConfiguration
{
    public static void ConfigureMetrics(MeterProviderBuilder m, ObservabilityOptions options)
    {
        m.AddMeter(options.Metrics?.MeterName ?? "HealthSyncApi")
            .AddRuntimeInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation();

        switch (options)
        {
            case { Metrics.ExportToPrometheus: true }:
                {
                    // TODO: Implement Prometheus exporter configuration
                }
                break;
            default:
                m.AddConsoleExporter();
                break;
        }
    }

    public sealed class InstrumentationSource : IDisposable
    {
        public const string ActivitySourceName = "HealthSyncApi";
        public const string MeterName = "HealthSyncApi";
        private readonly Meter meter;
        private bool _disposed;

        public InstrumentationSource()
        {
            var version = typeof(InstrumentationSource).Assembly.GetName().Version?.ToString();

            ActivitySource = new ActivitySource(ActivitySourceName, version);

            meter = new Meter(MeterName, version);

            RequestCounter = meter.CreateCounter<long>(
                "http_requests_total",
                description: "Total number of HTTP requests"
            );
        }

        public ActivitySource ActivitySource { get; }
        public Counter<long> RequestCounter { get; }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // Only call dispose on the Meter and ActivitySource if they implement IDisposable, otherwise just ignore it.
            meter.Dispose();
            ActivitySource.Dispose();
        }
    }
}
