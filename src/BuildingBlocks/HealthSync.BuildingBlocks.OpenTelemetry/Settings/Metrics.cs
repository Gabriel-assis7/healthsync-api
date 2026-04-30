namespace HealthSync.BuildingBlocks.OpenTelemetry.Settings;

public class MetricsOptions
{
    public bool Enabled { get; set; } = true;

    public bool ExportToPrometheus { get; set; } = true;
    public string? PrometheusEndpoint { get; set; }

    public string? MeterName { get; set; }

    public string? ApiVersion { get; set; }
}
