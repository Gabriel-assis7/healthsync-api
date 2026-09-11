using HealthSync.BuildingBlocks.Core.Domain.Aggregate;

namespace HealthSync.BuildingBlocks.Core.Domain.Repository.EfWrite;

public abstract class EfWriteRepository<TAggregateRoot, TAggregateRootId> : IWriteRepository<TAggregateRoot, TAggregateRootId>
    where TAggregateRoot : IAggregateRoot
    where TAggregateRootId : IEquatable<TAggregateRootId>
{
    protected DbSet<TAggregateRoot> Aggregates { get; }

    public virtual TAggregateRoot Insert(TAggregateRoot entity)
    {
        Aggregates.Add(entity);
        return entity;
    }

    public virtual async Task<TAggregateRoot> InsertAsync(TAggregateRoot entity)
    {
        await Aggregates.AddAsync(entity);
        return entity;
    }

    public virtual TAggregateRoot Update(TAggregateRoot entity)
    {
        Aggregates.Update(entity);
        return entity;
    }

    public virtual Task<TAggregateRoot> UpdateAsync(TAggregateRoot entity)
    {
        return Task.FromResult(Update(entity));
    }

    public virtual void Delete(TAggregateRoot entity)
    {
        Aggregates.Remove(entity);
    }

    public virtual Task DeleteAsync(TAggregateRoot entity)
    {
        Delete(entity);
        return Task.CompletedTask;
    }
}