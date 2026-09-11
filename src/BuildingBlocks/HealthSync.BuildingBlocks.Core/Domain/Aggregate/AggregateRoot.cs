using HealthSync.BuildingBlocks.Abstraction.Events;
using HealthSync.BuildingBlocks.Core.Domain.Entities;

namespace HealthSync.BuildingBlocks.Core.Domain.Aggregate;

[Serializable]
public abstract class AggregateRoot : BaseEntity, IAggregateRoot
{
    private readonly List<IAggregateEvent> _aggregateEvents = [];

    public void AddAggregateEvent(IAggregateEvent @event)
    {
        _aggregateEvents.Add(@event);
    }

    public IReadOnlyCollection<IAggregateEvent> GetAggregateEvents()
        => _aggregateEvents.AsReadOnly();

    public void ClearAggregateEvents()
    {
        _aggregateEvents.Clear();
    }
}