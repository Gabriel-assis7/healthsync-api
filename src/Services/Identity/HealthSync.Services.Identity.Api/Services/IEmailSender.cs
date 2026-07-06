namespace HealthSync.Services.Identity.Api.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string message, CancellationToken cancellationToken = default);
}