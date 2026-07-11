using BuildingBlocks.Abstractions.Diagnostics.Data;

namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics;

public interface IActivityRunner
{
    Activity? CreateAndStartActivity(ActivityData activityInfo);

    Task ExecuteActivityAsync(
        ActivityData activityInfo,
        Func<Activity?, CancellationToken, Task> action,
        CancellationToken cancellationToken = default
    );

    Task<TResult?> ExecuteActivityAsync<TResult>(
        ActivityData activityInfo,
        Func<Activity?, CancellationToken, Task<TResult>> action,
        CancellationToken cancellationToken = default
    );
}