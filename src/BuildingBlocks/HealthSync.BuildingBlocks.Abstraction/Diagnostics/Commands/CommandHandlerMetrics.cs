using System.Diagnostics;
using System.Diagnostics.Metrics;
using HealthSync.BuildingBlocks.Abstraction.Commands;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Constants;

namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics.Commands;

public class CommandHandlerMetrics
{
    private readonly UpDownCounter<long> _activeCommandsCounter;
    private readonly Counter<long> _totalCommandsCounter;
    private readonly Counter<long> _successCommandsCounter;
    private readonly Counter<long> _failedCommandsCounter;
    private readonly Histogram<double> _handlerDuration;
    private readonly Meter _meter;
    private Stopwatch? _timer;

    public CommandHandlerMetrics()
    {
        _meter = new Meter(TelemetryTags.InstrumentationName);

        _activeCommandsCounter = _meter.CreateUpDownCounter<long>(
            TelemetryTags.Metrics.Application.Commands.activeCount,
            unit: "{active_commands}",
            description: "Number of commands currently being handled"
        );

        _totalCommandsCounter = _meter.CreateCounter<long>(
            TelemetryTags.Metrics.Application.Commands.totalExecutedCount,
            unit: "{total_commands}",
            description: "Total number of executed commands"
        );

        _successCommandsCounter = _meter.CreateCounter<long>(
            TelemetryTags.Metrics.Application.Commands.successCount,
            unit: "{success_commands}",
            description: "Number of commands handled successfully"
        );

        _failedCommandsCounter = _meter.CreateCounter<long>(
            TelemetryTags.Metrics.Application.Commands.failedCount,
            unit: "{failed_commands}",
            description: "Number of commands handled with errors"
        );

        _handlerDuration = _meter.CreateHistogram<double>(
            TelemetryTags.Metrics.Application.Commands.handlerDuration,
            unit: "s",
            description: "Measures the duration of command handlers"
        );
    }

    public void StartExecuting<TCommand>()
    {
        var tags = CreateTags<TCommand>();

        if (_activeCommandsCounter.Enabled)
        {
            _activeCommandsCounter.Add(1, tags);
        }

        if (_totalCommandsCounter.Enabled)
        {
            _totalCommandsCounter.Add(1, tags);
        }

        _timer = Stopwatch.StartNew();
    }

    public void FinishExecuting<TCommand>()
    {
        var tags = CreateTags<TCommand>();

        if (_activeCommandsCounter.Enabled)
        {
            _activeCommandsCounter.Add(-1, tags);
        }

        if (!_handlerDuration.Enabled)
        {
            return;
        }

        var elapsedSeconds = _timer?.Elapsed.TotalSeconds ?? 0;

        _handlerDuration.Record(elapsedSeconds, tags);

        if (_successCommandsCounter.Enabled)
        {
            _successCommandsCounter.Add(1, tags);
        }
    }

    public void FailedCommand<TCommand>()
    {
        var tags = CreateTags<TCommand>();

        if (_failedCommandsCounter.Enabled)
        {
            _failedCommandsCounter.Add(1, tags);
        }
    }

    private static TagList CreateTags<TCommand>()
    {
        var commandName = typeof(TCommand).Name;
        var handlerType = typeof(TCommand)
            .Assembly
            .GetTypes()
            .FirstOrDefault(t => t.GetInterfaces()
                .Any(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                    && i.GetGenericArguments()[0] == typeof(TCommand)));

        var commandHandlerName = handlerType?.Name;

        return new TagList
        {
            { TelemetryTags.Tracing.Application.Command.name, commandName },
            { TelemetryTags.Tracing.Application.Command.type, typeof(TCommand).FullName },
            { TelemetryTags.Tracing.Application.Command.handler, commandHandlerName },
            { TelemetryTags.Tracing.Application.Command.handlerType, handlerType?.FullName },
        };
    }
}
