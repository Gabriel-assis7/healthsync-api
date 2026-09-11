namespace HealthSync.BuildingBlocks.Core.Persistence.EfCore;

using System.Collections.Immutable;
using System.Data;
using System.Linq.Expressions;
using HealthSync.BuildingBlocks.Abstraction.Events;
using HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;
using HealthSync.BuildingBlocks.Core.Domain.Aggregate;
using HealthSync.BuildingBlocks.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public abstract class DbContextBase(DbContextOptions options) : DbContext(options), IDbContext
{
    private IDbContextTransaction? _currentTransaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        AddingSoftDeletes(modelBuilder);
        AddingVersioning(modelBuilder);
    }

    private static void AddingVersioning(ModelBuilder builder)
    {
        var types = builder.Model.GetEntityTypes()
            .Where(entityType => entityType.ClrType.IsAssignableTo(typeof(IVersionedEntity)));

        foreach (var entityType in types)
        {
            builder.Entity(entityType.ClrType)
                .Property(nameof(IVersionedEntity.Version))
                .IsRowVersion();
        }
    }

    private static void AddingSoftDeletes(ModelBuilder builder)
    {
        var types = builder.Model.GetEntityTypes()
            .Where(entityType => entityType.ClrType.IsAssignableTo(typeof(ISoftDelete)));

        foreach (var entityType in types)
        {
            var parameter = Expression.Parameter(entityType.ClrType, "entity");
            var isDeleted = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var filter = Expression.Lambda(
                Expression.Equal(isDeleted, Expression.Constant(false)),
                parameter);

            builder.Entity(entityType.ClrType).HasQueryFilter(filter);
        }
    }

    public async Task BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken = default)
    {
        _currentTransaction ??= await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeCurrentTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            await DisposeCurrentTransactionAsync();
        }
    }

    public IReadOnlyList<IAggregateEvent> DequeueUncommittedDomainEvents()
    {
        var aggregateEvents = ChangeTracker
            .Entries<IAggregateRoot>()
            .SelectMany(entry => entry.Entity.GetAggregateEvents())
            .ToImmutableList();

        foreach (var entry in ChangeTracker.Entries<IAggregateRoot>())
        {
            entry.Entity.ClearAggregateEvents();
        }

        return aggregateEvents;
    }

    public Task ExecuteTransactionalAsync(
        Func<Task> action,
        CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public Task<TResult> ExecuteTransactionalAsync<TResult>(
        Func<Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await action();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    private async ValueTask DisposeCurrentTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task RetryOnExceptionAsync(Func<Task> operation)
    {
        return Database.CreateExecutionStrategy().ExecuteAsync(operation);
    }
}

