using HealthSync.BuildingBlocks.Abstraction.Domain.Entities;
using HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;

namespace HealthSync.BuildingBlocks.Abstraction.Persistence;

public interface IRepository<TEntity, in TId> : IReadRepository<TEntity, TId>, IWriteRepository<TEntity, TId>, IDisposable
    where TEntity : class, IEntity<TId>
{
}