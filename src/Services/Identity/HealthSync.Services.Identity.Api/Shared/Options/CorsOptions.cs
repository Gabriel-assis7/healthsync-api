namespace HealthSync.Services.Identity.Api.Shared.Options;

public sealed class CorsOptions
{
    public string[] AllowedOrigins { get; set; } = [];
}
