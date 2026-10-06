namespace HealthSync.BuildingBlocks.Abstraction.Events;

public interface IDomainEventsAccessor
{
    IReadOnlyList<IDomainEvent> DequeueUncommittedDomainEvents();
}