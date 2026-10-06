using System.Linq.Expressions;
using HealthSync.BuildingBlocks.Abstraction.Domain.Entities;

namespace HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;

public interface IWriteRepository<TEntity, in TId>
    where TEntity : class, IEntity<TId>
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}