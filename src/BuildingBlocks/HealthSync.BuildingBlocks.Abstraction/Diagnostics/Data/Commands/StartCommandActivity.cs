using System.Diagnostics;
using BuildingBlocks.Abstractions.Diagnostics.Data;
using HealthSync.BuildingBlocks.Abstraction.Commands;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Constants;

namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics.Data.Commands;

public class StartCommandActivity(IActivityRunner activityRunner)
{
    public async Task ExecuteCommandAsync<TCommand>(
        TCommand command,
        CancellationToken cancellationToken
        ) where TCommand : class
    {
        var commandName = typeof(TCommand).Name;

        var handlerType = typeof(TCommand).Assembly.GetTypes()
        .FirstOrDefault(t => t.GetInterfaces()
        .Any(i => i.IsGenericType
        && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
        && i.GetGenericArguments()[0] == typeof(TCommand)));

        await activityRunner.ExecuteActivityAsync(
            new ActivityData
            {
                ActivityKind = ActivityKind.Internal,
                Tags = new Dictionary<string, object?>
                {
                    { TelemetryTags.Tracing.Application.Command.Name, commandName },
                    { TelemetryTags.Tracing.Application.Command.Type, typeof(TCommand).FullName ?? "Unknown" },
                    { TelemetryTags.Tracing.Application.Command.Handler, handlerType?.Name ?? "Unknown" },
                    { TelemetryTags.Tracing.Application.Command.HandlerType, handlerType?.FullName ?? "Unknown" },
                    { TelemetryTags.Tracing.Application.Command.CorrelationId, Guid.NewGuid().ToString() },
                    { TelemetryTags.Tracing.Application.Command.CausationId, Guid.NewGuid().ToString() },
                    { TelemetryTags.Tracing.Application.Command.TenantId, "default-tenant" },
                    { TelemetryTags.Tracing.Application.Command.UserId, "default-user" },
                },
            },
            command,
            cancellationToken
            );
    }
}