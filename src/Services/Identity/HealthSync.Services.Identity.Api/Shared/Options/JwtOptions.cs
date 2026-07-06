namespace HealthSync.Services.Identity.Api.Shared.Options
{
    using System.ComponentModel.DataAnnotations;

    public sealed class JwtOptions
    {
        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Required]
        public string PrivateKey { get; set; } = string.Empty;

        [Required]
        public string PublicKey { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int TokenExpiry { get; set; } = 15;
    }
}
