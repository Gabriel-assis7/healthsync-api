using HealthSync.BuildingBlocks.Abstraction.Exceptions.Authentication;
using HealthSync.BuildingBlocks.Abstraction.Exceptions.Validation;
using HealthSync.Services.Identity.Api.Dto;
using HealthSync.Services.Identity.Api.Exceptions.Verification;
using HealthSync.Services.Identity.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HealthSync.Services.Identity.Api.Services;

public class LoginServiceEfCore(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
IEmailSender emailSender, ITokenService tokenService) : ILoginService<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<LoginResponse> LoginAsync(LoginRequest req, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(req.Email) ?? throw new EmailNotFoundException(req.Email);

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new InvalidOperationException("The user account does not have an email address configured.");
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            throw new EmailNotVerifiedException(user.Email);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            throw new LockedOutException();
        }

        if (result.RequiresTwoFactor)
        {
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            var msg = $"Your verification code is: {code}. Please enter this code to complete your login process.";

            if (string.IsNullOrEmpty(code))
            {
                throw new InvalidOperationException("Failed to generate two-factor authentication code.");
            }

            await _emailSender.SendEmailAsync(
                user.Email,
                "Two-Factor Authentication Code",
                msg,
                cancellationToken);

            return new LoginResponse
            {
                IsTwoFactorEnabled = true,
                Message = "Two-factor authentication is enabled. A verification code has been sent to your email.",
                UserId = user.Id,
                IsSuccess = false
            };
        }

        if (result.IsNotAllowed || !result.Succeeded)
        {
            throw new InvalidCredentialsException();
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName ?? user.Email)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var accessToken = await _tokenService.GenerateAccessToken(claims);
        var refreshToken = await _tokenService.GenerateRefreshToken();

        return new LoginResponse
        {
            IsSuccess = true,
            AccessToken = accessToken.AccessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Message = "Login successful."
        };
    }
}