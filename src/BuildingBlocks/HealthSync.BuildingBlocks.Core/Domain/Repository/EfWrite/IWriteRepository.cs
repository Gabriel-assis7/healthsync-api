using HealthSync.BuildingBlocks.Core.Domain.Aggregate;

namespace HealthSync.BuildingBlocks.Core.Domain.Repository.EfWrite;

public interface IWriteRepository<TAggregateRoot, in TAggregateRootId>
    where TAggregateRoot : IAggregateRoot
    where TAggregateRootId : IEquatable<TAggregateRootId>
{
    TAggregateRoot Get(TAggregateRootId id);
    Task<TAggregateRoot> GetAsync(TAggregateRootId id);
    TAggregateRoot InsertEntity(TAggregateRoot entity);
    Task<TAggregateRoot> InsertEntityAsync(TAggregateRoot entity);
    TAggregateRoot UpdateEntity(TAggregateRoot entity);
    Task<TAggregateRoot> UpdateEntityAsync(TAggregateRoot entity);
    void DeleteEntity(TAggregateRoot entity);
    Task DeleteEntityAsync(TAggregateRoot entity);
    void Delete(TAggregateRootId id);
    Task DeleteAsync(TAggregateRootId id);
}