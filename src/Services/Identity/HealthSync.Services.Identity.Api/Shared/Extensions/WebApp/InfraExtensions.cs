using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace HealthSync.Services.Identity.Shared.Extensions.WebApp;

public static class WebApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseDefaultCors();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Lax,
            Secure = CookieSecurePolicy.Always
        });

        app.UseAuthentication();
        app.UseAuthorization();
    }
}