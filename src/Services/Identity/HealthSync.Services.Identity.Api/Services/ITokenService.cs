using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace HealthSync.Services.Identity.Api.Services
{
    public interface ITokenService
    {
        Task<AccessTokenResponse> GenerateAccessToken(IEnumerable<Claim> claims);
        Task<string> GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}