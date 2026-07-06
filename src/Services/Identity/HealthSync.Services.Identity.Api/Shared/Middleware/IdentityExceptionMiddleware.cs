using System.Net;
using HealthSync.Services.Identity.Api.Exceptions.Verification;

namespace HealthSync.Services.Identity.Api.Shared.Middleware;

public sealed class IdentityExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
        : ExceptionHandlingMiddleware(next, logger)
{
    public override (HttpStatusCode StatusCode, string Message)
        GetResponse(Exception exception)
    {
        return exception switch
        {
            EmailAlreadyExistsException =>
                (HttpStatusCode.Conflict,
                 "The provided email address is already in use."),
            EmailNotVerifiedException =>
                (HttpStatusCode.Forbidden,
                 "The email address has not been verified. Please verify your email before proceeding."),
            EmailAlreadyVerifiedException =>
                (HttpStatusCode.Conflict,
                 "The email address has already been verified."),
            EmailNotFoundException =>
                (HttpStatusCode.NotFound,
                 "The provided email address was not found. Please check the email and try again."),

            _ =>
                (HttpStatusCode.InternalServerError,
                 "An unexpected error occurred.")
        };
    }
}