namespace HealthSync.BuildingBlocks.Abstraction.Events;

public interface IDomainEvent : IEvent
{
    dynamic? AggregateId { get; }
    long AggregateVersion { get; }
}