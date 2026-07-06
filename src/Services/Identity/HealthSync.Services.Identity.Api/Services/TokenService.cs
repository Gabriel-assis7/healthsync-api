

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HealthSync.Services.Identity.Api.Shared.Options;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HealthSync.Services.Identity.Api.Services;

public sealed class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public Task<AccessTokenResponse> GenerateAccessToken(IEnumerable<Claim> claims)
    {
        ArgumentNullException.ThrowIfNull(claims);

        var now = DateTime.UtcNow;
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingCredentials = new SigningCredentials(CreateSigningSecurityKey(), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMinutes(_jwtOptions.TokenExpiry),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = signingCredentials
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Task.FromResult(new AccessTokenResponse
        {
            AccessToken = jwt,
            ExpiresIn = _jwtOptions.TokenExpiry * 60,
            RefreshToken = string.Empty
        });
    }

    public Task<string> GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Task.FromResult(Base64UrlEncoder.Encode(bytes));
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = CreateValidationSecurityKey(),
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        return tokenHandler.ValidateToken(token, validationParameters, out _);
    }

    private SymmetricSecurityKey CreateSigningSecurityKey()
    {
        if (string.IsNullOrWhiteSpace(_jwtOptions.PrivateKey))
        {
            throw new InvalidOperationException("JWT private key is not configured.");
        }

        return new SymmetricSecurityKey(CreateKeyMaterial(_jwtOptions.PrivateKey));
    }

    private SymmetricSecurityKey CreateValidationSecurityKey()
    {
        if (string.IsNullOrWhiteSpace(_jwtOptions.PublicKey))
        {
            return CreateSigningSecurityKey();
        }

        return new SymmetricSecurityKey(CreateKeyMaterial(_jwtOptions.PublicKey));
    }

    private static byte[] CreateKeyMaterial(string secret)
    {
        return Encoding.UTF8.GetBytes(secret);
    }
}