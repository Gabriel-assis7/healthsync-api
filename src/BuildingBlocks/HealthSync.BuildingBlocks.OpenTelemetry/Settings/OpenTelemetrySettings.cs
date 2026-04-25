namespace HealthSync.BuildingBlocks.OpenTelemetry.Settings
{
    public class ObservabilityOptions
    {
        public bool Enabled { get; set; } = true;
        public string ServiceName { get; set; } = string.Empty;
        public string? ServiceVersion { get; set; }
        public string? OtlpEndpoint { get; set; }
        public string[]? Sources { get; set; }
        public JaegerSettings? Jaeger { get; set; }
        public bool ExportToJaeger { get; set; } = true;
    }
}
