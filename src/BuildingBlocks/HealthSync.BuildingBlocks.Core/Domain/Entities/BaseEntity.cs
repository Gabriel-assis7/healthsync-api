using HealthSync.BuildingBlocks.Abstraction.Events;

namespace HealthSync.BuildingBlocks.Core.Domain.Entities;

public abstract class BaseEntity : IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public Guid Id { get; set; }

    public DateTime Created { get; init; } = DateTime.UtcNow;

    public DateTime? Updated { get; set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents =>
        _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
    {
        var events = _domainEvents.ToArray();
        _domainEvents.Clear();

        return events;
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }
}