using System.Data;
using HealthSync.BuildingBlocks.Abstraction.Events;
using HealthSync.BuildingBlocks.Abstraction.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthSync.BuildingBlocks.Core.Persistence.EfCore;

// https://github.com/nhibernate/nhibernate-core/discussions/2731
// https://learn.microsoft.com/en-us/ef/core/saving/transactions#default-transaction-behavior
public class EfUnitOfWork<TDbContext>(
    TDbContext context,
    IDomainEventsAccessor domainEventsAccessor,
    IDomainEventPublisher domainEventPublisher
) : IEfUnitOfWork<TDbContext>
    where TDbContext : DbContextBase
{
    private bool disposedValue;

    public TDbContext DbContext => context;

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return context.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = domainEventsAccessor.DequeueUncommittedDomainEvents();
        await domainEventPublisher.PublishAsync(domainEvents.ToArray(), cancellationToken);

        await context.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return context.RollbackTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = domainEventsAccessor.DequeueUncommittedDomainEvents();
        await domainEventPublisher.PublishAsync(domainEvents.ToArray(), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    public DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return context.Set<TEntity>();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                context.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public Task RetryOnExceptionAsync(Func<Task> operation)
    {
        return context.RetryOnExceptionAsync(operation);
    }

    public Task<TResult> RetryOnExceptionAsync<TResult>(Func<Task<TResult>> operation)
    {
        return (Task<TResult>)context.RetryOnExceptionAsync(operation);
    }
}