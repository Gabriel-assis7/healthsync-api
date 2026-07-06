using SendGrid;
using SendGrid.Helpers.Mail;

namespace HealthSync.Services.Identity.Api.Services;

public class SendGridEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SendGridEmailSender> _logger;

    public SendGridEmailSender(
        IConfiguration configuration,
        ILogger<SendGridEmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task SendEmailAsync(string to, string subject, string message, CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];
        var fromEmail = _configuration["SendGrid:FromEmail"];
        
        var msg = new SendGridMessage
        {
            From = new EmailAddress(fromEmail),
            Subject = subject,
            PlainTextContent = message
        };
        msg.AddTo(new EmailAddress(to));

        if (_configuration.GetValue<bool>("SendGrid:SandBoxMode", false))
        {
            msg.MailSettings = new MailSettings
            {
                SandboxMode = new SandboxMode
                {
                    Enable = true
                }
            };
        }

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(fromEmail))
        {
            throw new InvalidOperationException("SendGrid API key or From email is not configured.");
        }

        var client = new SendGridClient(apiKey);

        _logger.LogInformation("Sending email to {To} with subject {Subject} with message {msg}", to, subject, msg.Serialize());
        var response = await client.SendEmailAsync(msg, cancellationToken);

        if (response == null)
        {
            _logger.LogError("SendGrid response is null.");
            throw new InvalidOperationException("SendGrid response is null.");
        }

        if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            _logger.LogError("Failed to send email via SendGrid. Status Code: {StatusCode}", response.StatusCode);
            throw new InvalidOperationException($"Failed to send email via SendGrid. Status Code: {response.StatusCode}");
        }

        _logger.LogInformation("SendGrid responded with {StatusCode}", response.StatusCode);

        var body = response.Body.ReadAsStringAsync(cancellationToken).Result;
        _logger.LogInformation("Response Body: {Body}", body);
    }
}