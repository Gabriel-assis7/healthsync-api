namespace HealthSync.BuildingBlocks.Abstraction.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : class
{
    TContext Context { get; }
}