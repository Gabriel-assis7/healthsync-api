namespace HealthSync.Services.Identity.Shared.Options;

public sealed class CorsOptions
{
    public string[] AllowedOrigins { get; set; } = [];
}
