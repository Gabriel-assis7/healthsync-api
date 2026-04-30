using HealthSync.BuildingBlocks.OpenTelemetry.Settings;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace HealthSync.BuildingBlocks.OpenTelemetry.Configuration
{
    // https://opentelemetry.io/docs/languages/dotnet/traces/getting-started-aspnetcore/
    // https://opentelemetry.io/docs/languages/dotnet/resources/#adding-resources-in-code
    public static class TracingConfiguration
    {
        public static void ConfigureTracing(TracerProviderBuilder t, ObservabilityOptions options)
        {
            t.AddAspNetCoreInstrumentation(opt =>
            {
                opt.Filter = context =>
                {
                    // Exclude endpoints from tracing
                    var basePath = context.Request.Path;

                    return !basePath.StartsWithSegments("/api/v1/health")
                        && !basePath.StartsWithSegments("/api/v1/metrics");
                };
            });
            t.AddHttpClientInstrumentation();

            t.SetSampler(new TraceIdRatioBasedSampler(options.SampleRate));

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
                default:
                    t.AddConsoleExporter();
                    break;
            }
            ;
        }
    }
}
