using System.Diagnostics;

namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics;

public class ActivityData
{
    public string? Name { get; set; }
    public string? Payload { get; set; }
    public IDictionary<string, object?> Tags { get; set; } = new Dictionary<string, object?>();
    public string? ParentId { get; set; }
    public ActivityContext? Parent { get; set; }
    public required ActivityKind ActivityKind { get; set; } = ActivityKind.Internal;
    public double DurationMilliseconds { get; set; }
}