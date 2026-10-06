using Microsoft.EntityFrameworkCore;

namespace HealthSync.BuildingBlocks.Abstraction.Persistence.UnitOfWork;

public interface IEfUnitOfWork : IUnitOfWork, ITransactionalUnitOfWork, IRetryableUnitOfWork
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
}

public interface IEfUnitOfWork<out TContext> : IEfUnitOfWork
    where TContext : DbContext
{
    TContext DbContext { get; }
}