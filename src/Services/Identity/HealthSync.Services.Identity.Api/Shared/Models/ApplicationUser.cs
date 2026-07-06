using Microsoft.AspNetCore.Identity;

namespace HealthSync.Services.Identity.Shared.Models;


public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTimeOffset? EmailConfirmedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public UserTwoFactor? TwoFactor { get; private set; }
}