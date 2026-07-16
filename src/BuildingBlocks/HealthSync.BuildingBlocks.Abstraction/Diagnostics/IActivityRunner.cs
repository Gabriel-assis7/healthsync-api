using System.Diagnostics;
namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics;

public interface IActivityRunner
{
    Task ExecuteActivityAsync(
        ActivityData activityInfo,
        Func<Activity?, CancellationToken, Task> commandAction,
        CancellationToken cancellationToken = default
    );

    Task<TResult> ExecuteActivityAsync<TResult>(
        ActivityData activityInfo,
        Func<Activity?, CancellationToken, Task<TResult>> commandAction,
        CancellationToken cancellationToken = default
    );
}