using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using HealthSync.BuildingBlocks.Abstraction.Domain.Entities;
using HealthSync.BuildingBlocks.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace HealthSync.BuildingBlocks.Core.Persistence.EfCore;

public abstract class EfRepositoryBase<TDbContext, TEntity, TKey>(
    TDbContext dbContext,
    ISieveProcessor sieveProcessor
) : IRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TKey>
{
    protected TDbContext DbContext { get; } = dbContext;
    protected ISieveProcessor SieveProcessor { get; } = sieveProcessor;

    public async Task<TEntity> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>()
            .AddAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        return entity;
    }

    public Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DbContext.Set<TEntity>().Update(entity);
        return Task.FromResult(entity);
    }

    public Task DeleteRangeAsync(
        IReadOnlyList<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var entity in entities)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var entities = await DbContext.Set<TEntity>()
            .Where(predicate)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        DbContext.Set<TEntity>().RemoveRange(entities);
    }

    public Task DeleteAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DbContext.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        var entity = await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(entity => entity.Id!.Equals(id), cancellationToken)
            .ConfigureAwait(false);

        if (entity is not null)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }
    }

    public async Task<TEntity?> FindByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id!.Equals(id), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>().AsNoTracking(),
            specification);

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TResult>> GetAllAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>().AsNoTracking(),
            specification);

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>().AsNoTracking(),
            specification);

        return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>().AsNoTracking(),
            specification);

        return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(
        ISingleResultSpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>().AsNoTracking(),
            specification);

        return await query.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResult?> SingleOrDefaultAsync<TResult>(
        ISingleResultSpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>().AsNoTracking(),
            specification);

        return await query.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CountAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>(),
            specification,
            evaluateCriteriaOnly: true);

        return await query.CountAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> AnyAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator.Default.GetQuery(
            DbContext.Set<TEntity>(),
            specification,
            evaluateCriteriaOnly: true);

        return await query.AnyAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .AnyAsync(predicate, cancellationToken)
            .ConfigureAwait(false);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
