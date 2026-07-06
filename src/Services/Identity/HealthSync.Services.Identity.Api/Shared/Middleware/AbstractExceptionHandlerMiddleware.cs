using System.Net;
using Microsoft.Extensions.Logging;

namespace HealthSync.Services.Identity.Api.Shared.Middleware;

public abstract class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public abstract (HttpStatusCode StatusCode, string Message) GetResponse(Exception exception);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var (statusCode, message) = GetResponse(ex);

            _logger.LogError(ex, "An unhandled exception occurred while processing the request. StatusCode: {StatusCode}, Message: {Message}, Path: {Path}", statusCode, message, context.Request.Path.Value);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new { error = message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }

}