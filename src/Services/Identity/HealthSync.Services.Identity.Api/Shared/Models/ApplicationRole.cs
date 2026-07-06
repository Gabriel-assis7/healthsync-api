using Microsoft.AspNetCore.Identity;

namespace HealthSync.Services.Identity.Shared.Models;


public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}