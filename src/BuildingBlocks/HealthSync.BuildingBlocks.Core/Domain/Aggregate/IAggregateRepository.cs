using HealthSync.BuildingBlocks.Abstraction.Events;
using HealthSync.BuildingBlocks.Core.Domain.Entities;

namespace HealthSync.BuildingBlocks.Core.Domain.Aggregate;

public interface IAggregateRepository<TIEntity>
    where TIEntity : IEntity
{
    Guid Id { get; }
    Task<TIEntity?> LoadState(Guid id);
    Task UpdateAndSaveState(
    IAggregateEvent aggregateEvent);
}