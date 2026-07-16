using System.Diagnostics;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Commands;
using HealthSync.BuildingBlocks.Abstraction.Diagnostics.Constants;
using MediatR;

namespace HealthSync.BuildingBlocks.Abstraction.Behaviors
{
    public class DiagnosticsPipeline<TRequest, TResponse>(
        StartCommandActivity startCommandActivity,
        CommandHandlerMetrics commandHandlerMetrics
    ) : DiagnosticPipelineBase, IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var sw = new Stopwatch();

            commandHandlerMetrics.StartExecuting<TRequest>();

            if (_logger.IsEnabled(DiagnosticListenerConstants.RequestStarted))
            {
                _logger.Write(DiagnosticListenerConstants.RequestStarted, new
                {
                    Id = id,
                    RequestType = GetRequestType(request),
                    RequestName = request.GetType().FullName,
                    Request = request,
                });
            }

            try
            {
                sw.Start();

                var commandResult = await startCommandActivity.ExecuteCommandAsync(
                    request,
                    _ => next(),
                    cancellationToken
                ).ConfigureAwait(false);

                sw.Stop();

                commandHandlerMetrics.FinishExecuting<TRequest>();

                if (_logger.IsEnabled(DiagnosticListenerConstants.RequestCompleted))
                {
                    _logger.Write(DiagnosticListenerConstants.RequestCompleted, new
                    {
                        Id = id,
                        RequestType = GetRequestType(request),
                        RequestName = request.GetType().FullName,
                        Request = request,
                        Response = commandResult,
                        Duration = sw.Elapsed,
                    });
                }

                return commandResult;
            }
            catch (Exception e)
            {
                sw.Stop();

                commandHandlerMetrics.FailedCommand<TRequest>();

                if (_logger.IsEnabled(DiagnosticListenerConstants.RequestError))
                {
                    _logger.Write(DiagnosticListenerConstants.RequestError, new
                    {
                        Id = id,
                        RequestType = GetRequestType(request),
                        RequestName = request.GetType().FullName,
                        Request = request,
                        Exception = e,
                        Duration = sw.Elapsed,
                    });
                }

                throw;
            }
            finally
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }
            }
        }

        private static string GetRequestType(TRequest request)
        {
            return request?.GetType().Name ?? "Unknown";
        }
    }
}
