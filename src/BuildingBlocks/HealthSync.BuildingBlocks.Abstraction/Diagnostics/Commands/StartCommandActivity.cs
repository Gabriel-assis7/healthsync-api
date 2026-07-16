using System.Diagnostics;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics;
using HealthSync.BuildingBlocks.Abstraction.Commands;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Constants;

namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics.Commands;

public class StartCommandActivity(IActivityRunner activityRunner)
{
    public async Task<TResponse> ExecuteCommandAsync<TCommand, TResponse>(
        TCommand command,
        Func<CancellationToken, Task<TResponse>> commandAction,
        CancellationToken cancellationToken) where TResponse : notnull
    {
        var commandName = typeof(TCommand).Name;

        var handlerType = typeof(TCommand).Assembly.GetTypes()
            .FirstOrDefault(t => t.GetInterfaces()
                .Any(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                    && i.GetGenericArguments()[0] == typeof(TCommand)));

        var response = await activityRunner.ExecuteActivityAsync(
            new ActivityData
            {
                ActivityKind = ActivityKind.Internal,
                Tags = new Dictionary<string, object?>
                {
                    { TelemetryTags.Tracing.Application.Command.name, commandName },
                    { TelemetryTags.Tracing.Application.Command.type, typeof(TCommand).Name },
                    { TelemetryTags.Tracing.Application.Command.handler, handlerType?.Name ?? "Unknown"},
                    { TelemetryTags.Tracing.Application.Command.handlerType, handlerType?.FullName}
                }
            },
            async (_, ct) => await commandAction(ct),
            cancellationToken);

        return response;
    }
}