namespace HealthSync.BuildingBlocks.Abstraction.Persistence.UnitOfWork;

public interface ITransactionalUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}