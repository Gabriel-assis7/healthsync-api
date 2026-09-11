namespace HealthSync.BuildingBlocks.Abstraction.Events;

public interface INotification { }

public interface IEvent : INotification
{
    Guid Id { get; }
    long EventVersion { get; }
    DateTimeOffset TimeStamp { get; }
    public string? EventType { get; }
    public DateTime CreatedAt { get; }
    public string? CorrelationId { get; init; }
    public IDictionary<string, object> MetaData { get; }
}