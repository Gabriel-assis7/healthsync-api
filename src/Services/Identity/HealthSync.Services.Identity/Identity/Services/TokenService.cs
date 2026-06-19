using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using HealthSync.Services.Identity.Shared.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HealthSync.Services.Identity.Identity.Services;

public sealed class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    private readonly SigningCredentials _signingCredentials =
        new(CreateRsaSecurityKey(jwtOptions.Value.PrivateKey),
            SecurityAlgorithms.RsaSha256);

    private readonly TokenValidationParameters _expiredTokenValidationParameters =
        new()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = CreateRsaSecurityKey(jwtOptions.Value.PublicKey),
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_jwt.TokenExpiry),
            signingCredentials: _signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(secureRandomBytes);
        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var principal = new JwtSecurityTokenHandler().ValidateToken(token, _expiredTokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.OrdinalIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token.");
        }

        return principal;
    }

    private static RsaSecurityKey CreateRsaSecurityKey(string keyPem)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(keyPem);
        return new RsaSecurityKey(rsa);
    }
}