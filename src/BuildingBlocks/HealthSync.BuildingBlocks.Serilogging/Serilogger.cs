namespace HealthSync.BuildingBlocks.Serilogging;

public sealed class SerilogOptions
{
    public bool Enabled { get; set; }
    public string? ElasticSearchUrl { get; set; }
    public bool UseConsole { get; set; } = true;
    public bool ExportToOpenTelemetry { get; set; } = true;

    public string? SeqUrl { get; set; }
}
