using System.Linq.Expressions;
using Ardalis.Specification;
using HealthSync.BuildingBlocks.Abstraction.Domain.Entities;

namespace HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;

public interface IReadRepository<TEntity, in TId>
    where TEntity : class, IEntity<TId>
{
    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default
    );

    Task<TEntity?> FirstOrDefaultAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    );

    Task<TResult?> FirstOrDefaultAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default
    );

    Task<TEntity?> SingleOrDefaultAsync(
        ISingleResultSpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    );

    Task<TResult?> SingleOrDefaultAsync<TResult>(
        ISingleResultSpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default
    );

    Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}