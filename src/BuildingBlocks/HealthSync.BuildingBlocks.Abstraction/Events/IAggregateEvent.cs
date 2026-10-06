namespace HealthSync.BuildingBlocks.Abstraction.Events;

public interface IAggregateEvent
{
    Guid Id { get; }
}