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
            private const string consumer = "application.consumer";
            private const string producer = "application.producer";

            public static string Consumer => consumer;

            public static string Producer => producer;

            public static class Command
            {
                private const string name = "command.name";
                private const string type = "command.type";

                private const string handler = "command.handler";
                private const string handlerType = "command.handler.type";

                private const string correlationId = "command.correlation.id";
                private const string causationId = "command.causation.id";

                private const string tenantId = "command.tenant.id";
                private const string userId = "command.user.id";

                public static string Name => name;

                public static string Type => type;

                public static string Handler => handler;

                public static string HandlerType => handlerType;

                public static string CorrelationId => correlationId;

                public static string CausationId => causationId;

                public static string TenantId => tenantId;

                public static string UserId => userId;
            }
        }

        public static class Exception
        {
            public const string eventName = "exception";
            public const string type = "exception.type";
            public const string message = "exception.message";
            public const string stacktrace = "exception.stacktrace";

            public static string Event => eventName;
            public static string ExceptionType => type;
            public static string ExceptionMessage => message;
            public static string ExceptionStacktrace => stacktrace;
        }
    }
}