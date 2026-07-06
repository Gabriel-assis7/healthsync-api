using Microsoft.AspNetCore.Identity;

namespace HealthSync.Services.Identity.Shared.Models;


public class UserTwoFactor : IdentityUser<Guid>
{
    public Guid UserId { get; private set; }

    public bool IsEnabled { get; private set; }

    public string? TotpSecret { get; private set; }

    public long? LastUsedTimeWindow { get; private set; }

    public DateTimeOffset? EnabledAt { get; private set; }

    public ApplicationUser User { get; private set; } = default!;
}