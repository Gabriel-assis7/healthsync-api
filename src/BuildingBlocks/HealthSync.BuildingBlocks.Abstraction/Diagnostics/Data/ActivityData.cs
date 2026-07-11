namespace BuildingBlocks.Abstractions.Diagnostics.Data;

public class ActivityData
{
    public Guid RequestGuid { get; set; }
    public string? Payload { get; set; }
    public string? Name { get; set; }
    public IDictionary<string, object?> Tags { get; set; } = new Dictionary<string, object?>();
    public string? ParentId { get; set; }
    public ActivityContext? Parent { get; set; }
    public required ActivityKind ActivityKind = ActivityKind.Internal;
    public double DurationMilliseconds { get; set; }
}