namespace HealthSync.BuildingBlocks.Abstraction.Persistence.UnitOfWork;

public interface IRetryableUnitOfWork
{
    Task RetryOnExceptionAsync(Func<Task> operation);
    Task<TResult> RetryOnExceptionAsync<TResult>(Func<Task<TResult>> operation);
}
