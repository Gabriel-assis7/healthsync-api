namespace HealthSync.BuildingBlocks.Abstraction.Persistence.EfCore;

public interface IRetryDbContextExecution
{
    Task RetryOnExceptionAsync(Func<Task> operation);
}