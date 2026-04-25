namespace HealthSync.BuildingBlocks.OpenTelemetry.Settings;

public class JaegerSettings
{
    // gRPC: http://jaeger:4317
    // HTTP: http://jaeger:4318
    public string Endpoint { get; set; } = "http://localhost:4317";

    // grpc or http/protobuf
    public string Protocol { get; set; } = "grpc";

    public string? Headers { get; set; }
    public int? TimeoutInMilliseconds { get; set; }
}
