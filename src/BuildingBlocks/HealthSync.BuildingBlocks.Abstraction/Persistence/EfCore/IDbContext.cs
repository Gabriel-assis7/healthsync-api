using System.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;

public interface IDbContext : ITxDbContextExecute, IRetryDbContextExecution
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}