using HealthSync.BuildingBlocks.Abstraction.Events;

namespace HealthSync.BuildingBlocks.Abstraction.Domain.Aggregate;

public interface IAggregateRoot
{
    void AddAggregateEvent(IAggregateEvent @event);
    IReadOnlyCollection<IAggregateEvent> GetAggregateEvents();
    void ClearAggregateEvents();
}