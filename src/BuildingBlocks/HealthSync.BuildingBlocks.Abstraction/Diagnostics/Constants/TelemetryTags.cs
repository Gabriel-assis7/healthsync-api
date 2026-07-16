namespace HealthSync.BuildingBlocks.Abstraction.Diagnostics.Constants;

public static class TelemetryTags
{
    private static string _instrumentationName = "default-instrumentation";

    public static string InstrumentationName { get => _instrumentationName; set => _instrumentationName = value; }

    public static void Configure(string instrumentationName)
    {
        if (!string.IsNullOrEmpty(instrumentationName))
            InstrumentationName = instrumentationName;
    }

    public static class Tracing
    {
        public static class Application
        {
            public const string consumer = "application.consumer";
            public const string producer = "application.producer";
            public static class Command
            {
                public const string name = "command.name";
                public const string type = "command.type";

                public const string handler = "command.handler";
                public const string handlerType = "command.handler.type";
            }
        }

        public static class Exception
        {
            public const string eventName = "exception";
            public const string type = "exception.type";
            public const string message = "exception.message";
            public const string stacktrace = "exception.stacktrace";
        }
    }

    public static class Metrics
    {
        public static class Application
        {
            public static class Commands
            {
                public const string activeCount = "application.commands.active.count";
                public const string totalExecutedCount = "application.commands.total.count";
                public const string successCount = "application.commands.success.count";
                public const string failedCount = "application.commands.failed.count";
                public const string handlerDuration = "application.commands.handler.duration";
            }
        }
    }
}