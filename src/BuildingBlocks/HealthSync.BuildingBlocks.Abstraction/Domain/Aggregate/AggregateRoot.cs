using HealthSync.BuildingBlocks.Abstraction.Events;

namespace HealthSync.BuildingBlocks.Abstraction.Domain.Aggregate;

[Serializable]
public abstract class AggregateRoot : IAggregateRoot
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