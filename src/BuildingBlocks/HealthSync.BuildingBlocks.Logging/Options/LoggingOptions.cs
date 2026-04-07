namespace HealthSync.BuildingBlocks.Logging.Options;

public sealed class LoggingOptions
{
    public bool Enabled { get; set; }
    public string? ElasticSearchUrl { get; set; }
    public bool UseConsole { get; set; } = true;
    public bool ExportToOpenTelemetry { get; set; } = true;

    public string? SeqUrl { get; set; }
}
