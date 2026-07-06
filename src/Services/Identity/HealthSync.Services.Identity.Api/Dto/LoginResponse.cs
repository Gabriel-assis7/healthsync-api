namespace HealthSync.Services.Identity.Api.Dto;

public sealed record LoginResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public bool IsTwoFactorEnabled { get; set; }
    public string? TwoFactorToken { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public Guid UserId { get; set; }
}