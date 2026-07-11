using System.Diagnostics;
using Microsoft.Extensions.Logging;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Commands;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Constants;

namespace HealthSync.BuildingBlocks.Abstraction.Behaviors
{
    public class DiagnosticsPipeline<TRequest, TResponse>(
        ILogger<DiagnosticsPipelineBehavior<TRequest, TResponse>> logger,
        StartCommandActivity startCommandActivity
    ) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {

        private readonly ILogger<DiagnosticsPipelineBehavior<TRequest, TResponse>> _logger;

        public DiagnosticsPipelineBehavior(ILogger<DiagnosticsPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            var id = new Guid();
            var sw = new Stopwatch();
            var isCommand = request is ICommand;
            var isQuery = request is IQuery;

            try
            {
                if (isCommand)
                {
                    sw.Start();
                    _logger.Write(Tracing.Tracing.Application.Command.Name, typeof(TRequest).Name);

                    var result = await startCommandActivity.ExecuteCommandAsync(
                        async (TRequest command,
                        CancellationToken cancellationToken) =>
                        {
                            _logger.Write(Tracing.Tracing.Application.Command.Name, typeof(TRequest).Name);
                            return await next(request, cancellationToken);
                        }
                    );
                    sw.Stop();
                    return result;
                }
            }
            catch (Exception e)
            {
                sw.Stop();
                if (_logger.IsEnabled(TracingTags.Tracing.Exception.Event))
                {
                    _logger.Write(TracingTags.Tracing.Exception.Event, ActivityExceptionData(
                        id,
                        e,
                        GetRequestType(request),
                        sw.Elapsed.TotalMilliseconds
                    ));
                }

                throw;
            }

            sw.Stop();
            return await next(request, cancellationToken);
        }

        private static string GetRequestType(TRequest requestType)
        {
            if (requestType is null)
            {
                throw new ArgumentNullException(nameof(requestType));
            }

            return requestType switch
            {
                IQuery _ => "Query",
                ICommand _ => "Command",
                _ => "Unknown",
            };
        }
    }
}
